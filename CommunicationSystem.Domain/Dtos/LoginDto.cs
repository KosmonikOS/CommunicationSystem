
using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Password { get; set; }
    }
}
