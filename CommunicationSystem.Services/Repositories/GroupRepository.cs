using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CommunicationSystem.Services.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly CommunicationContext context;

        public GroupRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IQueryable<Group> GetGroups(Expression<Func<Group, bool>> expression = null)
        {
            var query = context.Groups.AsNoTracking();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return query;
        }
        public IQueryable<GroupUser> GetGroupMembers(Guid groupId)
        {
            return context.GroupUser.Where(x => x.GroupId == groupId)
                .AsNoTracking();
        }
        public void UpdateGroupMembers(IEnumerable<GroupMemberStateDto> members, Guid groupId)
        {
            context.GroupUser.AddRange(members
                .Where(x => x.State == DbEntityState.Added)
                .Select(x => new GroupUser()
                {
                    GroupId = groupId,
                    UserId = x.UserId
                }));
            context.GroupUser.RemoveRange(members
                .Where(x => x.State == DbEntityState.Deleted)
                .Select(x => new GroupUser()
                {
                    GroupId = groupId,
                    UserId = x.UserId
                }));
        }
        public void AddGroup(Group group)
        {
            context.Add(group);
        }
        public void UpdateGroup(Group group)
        {
            context.Update(group);
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

    }
}
