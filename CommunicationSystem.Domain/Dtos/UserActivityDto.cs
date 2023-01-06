using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class UserActivityDto
    {
        public int Id { get; set; }
        public UserActivityState Activity { get; set; }
    }
}
