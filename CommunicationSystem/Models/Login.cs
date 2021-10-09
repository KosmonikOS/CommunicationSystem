using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string  Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Password { get; set; }
    }
}
