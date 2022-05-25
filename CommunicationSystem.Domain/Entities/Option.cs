
namespace CommunicationSystem.Domain.Entities
{
    public class Option
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsRightOption { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
