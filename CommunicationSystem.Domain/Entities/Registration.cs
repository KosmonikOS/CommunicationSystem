using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Entities
{
    public class Registration
    {
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string NickName { get; set; }
    }
}
