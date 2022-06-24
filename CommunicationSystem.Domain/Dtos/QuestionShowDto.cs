using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class QuestionShowDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? Image { get; set; }
        public Guid TestId { get; set; }
        public IEnumerable<OptionShowDto> Options { get; set; }
    }
}
