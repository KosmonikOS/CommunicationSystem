
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.Dtos
{
    public class GroupMemberShowDto
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string AccountImage { get; set; }
    }
}
