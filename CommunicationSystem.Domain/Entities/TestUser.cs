
namespace CommunicationSystem.Domain.Entities
{
    public class TestUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public bool IsCompleted { get; set; }
        public int? Mark { get; set; }
    }
}
