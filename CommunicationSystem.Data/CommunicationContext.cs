using CommunicationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Data
{
    public class CommunicationContext: DbContext
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
        public CommunicationContext(DbContextOptions<CommunicationContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property("AccountImage")
                .HasDefaultValue("/assets/user.png");
            modelBuilder.Entity<User>().Property("RoleId")
                .HasDefaultValue(1);
            modelBuilder.Entity<Group>().Property("GroupImage")
                .HasDefaultValue("/assets/group.png");
            modelBuilder.Entity<Message>().HasOne(x => x.From)
                .WithMany().HasForeignKey("FromId");
            modelBuilder.Entity<Message>().HasOne(x => x.To)
                .WithMany().HasForeignKey("ToId");
            modelBuilder.Entity<Test>().HasOne(x => x.Creator)
                .WithMany().HasForeignKey("CreatorId");
            base.OnModelCreating(modelBuilder);
        }
    }
}
