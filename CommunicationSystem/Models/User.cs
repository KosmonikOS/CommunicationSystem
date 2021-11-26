using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string NickName { get; set; }
        [StringLength(50,MinimumLength =2, ErrorMessage ="Введите от 2 до 50 символов")]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string MiddleName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string LastName { get; set; }
        [Range(1,11,ErrorMessage ="Некорректный формат класса")]
        public int Grade { get; set; }
        public string GradeLetter { get; set; }
        public int Role { get; set; } = 1;
        [NotMapped]
        public string RoleName { get; set; }
        public string IsConfirmed { get; set; }
        public string accountImage { get; set; } = "/assets/user.png";
        [Phone(ErrorMessage ="Некорректный формат телефона")]
        public string Phone { get; set; }
    }
}
