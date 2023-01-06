using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Entities
{
    public class Question
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
        public Test Test { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
