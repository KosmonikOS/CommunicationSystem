using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class CreateOptionDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Text { get; set; }
        public bool IsRightOption { get; set; }
        public Guid QuestionId { get; set; }
    }
}
