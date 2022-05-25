
using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; } = 1;
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public string Image { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
