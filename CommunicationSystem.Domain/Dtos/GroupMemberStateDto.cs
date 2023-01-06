using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class GroupMemberStateDto
    {
        public int UserId { get; set; }
        public DbEntityState State { get; set; } = DbEntityState.Unchanged;
    }
}
