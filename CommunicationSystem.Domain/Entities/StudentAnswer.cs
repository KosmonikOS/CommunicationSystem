
namespace CommunicationSystem.Domain.Entities
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid TestId { get; set; }
        public Test Test {get;set;}
    }
}
