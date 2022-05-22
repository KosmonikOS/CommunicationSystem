
namespace CommunicationSystem.Domain.Entities
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public string Answer { get; set; }
    }
}
