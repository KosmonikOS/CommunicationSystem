using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class CreateTestDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Grade { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public int Time { get; set; }
        public int Creator { get; set; }
        public int Subject { get; set; }
        public ICollection<CreateQuestionDto> Questions { get; set; }
        public ICollection<TestStudentStateDto> Students { get; set; }
    }
}
