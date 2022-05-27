using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class UserAccountUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? LastName { get; set; }
        public int? Grade { get; set; }
        public string? GradeLetter { get; set; }
        public string AccountImage { get; set; }
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string? Phone { get; set; }  
        public string? Password { get; set;}
    }
}
