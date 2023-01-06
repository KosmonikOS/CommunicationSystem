
namespace CommunicationSystem.Domain.Entities
{
    public class GroupUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
