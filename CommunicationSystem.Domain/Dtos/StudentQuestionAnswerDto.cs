using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class StudentQuestionAnswerDto
    {
        public Guid Id { get; set; }
        public int Points { get; set; } = 1;
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public ICollection<string> Answers { get; set; }
    }
}