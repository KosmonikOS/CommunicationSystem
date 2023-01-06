
namespace CommunicationSystem.Domain.Dtos
{
    public class GroupShowDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GroupImage { get; set; }
        public ICollection<GroupMemberShowDto> Members { get; set; }
    }
}
