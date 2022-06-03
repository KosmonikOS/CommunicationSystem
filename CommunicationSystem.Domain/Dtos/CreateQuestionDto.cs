using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class CreateQuestionDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public int Points { get; set; } = 1;
        [Required(ErrorMessage = "Это поле обязательное")]
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public string? Image { get; set; }
        public Guid TestId { get; set; }
        public IEnumerable<CreateOptionDto> Options { get; set; }
    }
}
