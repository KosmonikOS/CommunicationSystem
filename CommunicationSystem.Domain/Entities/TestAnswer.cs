
namespace CommunicationSystem.Domain.Entities
{
    public class TestAnswer
    {
        public int TestId { get; set; }
        public int UserId { get; set; }
        public List<Question> Questions { get; set; }
    }
}
