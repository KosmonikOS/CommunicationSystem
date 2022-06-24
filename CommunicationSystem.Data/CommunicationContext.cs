using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Data
{
    public class CommunicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserSaltPass> UserSaltPass { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<TestUser> TestUser { get; set; }
        public DbSet<GroupUser> GroupUser { get; set; }

        //Raw Sql Entities
        public DbSet<ContactDto> Contacts { get; set; }
        public DbSet<ContactMessageDto> ContactMessages { get; set; }
        public DbSet<GroupMessageDto> GroupMessages { get; set; }
        public CommunicationContext(DbContextOptions<CommunicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<ContactMessageDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<GroupMessageDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<User>().Property("AccountImage")
                .HasDefaultValue("/assets/user.png");
            modelBuilder.Entity<User>().Property("RoleId")
                .HasDefaultValue(1);
            modelBuilder.Entity<User>().HasIndex(x => x.Email)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<User>().HasIndex(x => x.NickName)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<Role>().HasIndex(x => x.Name)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<Subject>().HasIndex(x => x.Name)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<Test>().HasIndex(x => x.Name)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<Test>().HasIndex(x => x.Grade)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
            modelBuilder.Entity<Test>().HasIndex(x => x.Date);
            modelBuilder.Entity<Group>().Property("GroupImage")
                .HasDefaultValue("/assets/group.png");
            modelBuilder.Entity<Message>().HasOne(x => x.From)
                .WithMany(x => x.FromMessages).HasForeignKey("FromId");
            modelBuilder.Entity<Message>().HasOne(x => x.To)
                .WithMany(x => x.ToMessages).HasForeignKey("ToId");
            modelBuilder.Entity<Message>().HasOne(x => x.Group)
                .WithMany().HasForeignKey("ToGroup");
            modelBuilder.Entity<Test>().HasOne(x => x.Creator)
                .WithMany(x => x.CreatedTests).HasForeignKey("CreatorId");
            modelBuilder.Entity<Test>().HasMany(x => x.Students)
                .WithMany(x => x.Tests).UsingEntity<TestUser>();
            modelBuilder.Entity<TestUser>()
                .HasKey(x => new { x.UserId, x.TestId });
            modelBuilder.Entity<Group>().HasMany(x => x.Users)
                .WithMany(x => x.Groups).UsingEntity<GroupUser>();
            modelBuilder.Entity<GroupUser>()
                .HasKey(x => new { x.UserId, x.GroupId });
            modelBuilder.HasDbFunction(() => UserFunctions.GetNotViewedMessages(default(int), default(int)));
            modelBuilder.HasDbFunction(() => UserFunctions.GetLastMessageDate(default(int), default(int)));
            modelBuilder.HasDbFunction(() => UserFunctions.GetLastMessage(default(int), default(int)));
            modelBuilder.HasDbFunction(() => UserFunctions.GetGroupLastMessageDate(default(Guid)));
            modelBuilder.HasDbFunction(() => UserFunctions.GetGroupLastMessage(default(Guid)));
            modelBuilder.HasDbFunction(() => UserFunctions.GetUserLastActivity(default(int)));
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("pg_trgm");
        }
    }
}
