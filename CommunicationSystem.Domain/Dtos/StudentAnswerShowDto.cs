
using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class StudentAnswerShowDto
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public int Points { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Text { get; set; }
        public string OpenAnswer { get; set; }
        public IEnumerable<StudentAnswerOptionShowDto> Options { get; set; }
    }
}
