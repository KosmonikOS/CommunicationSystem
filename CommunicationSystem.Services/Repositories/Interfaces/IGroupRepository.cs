using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using System.Linq.Expressions;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IGroupRepository :IBaseRepository
    {
        public IQueryable<Group> GetGroups(Expression<Func<Group, bool>> expression = null);
        public void AddGroup(Group group);
        public void UpdateGroup(Group group);
        public IQueryable<GroupUser> GetGroupMembers(Guid groupId);
        public void UpdateGroupMembers(IEnumerable<GroupMemberStateDto> members,Guid groupId);
    }
}
