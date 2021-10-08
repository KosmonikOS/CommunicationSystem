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
        [Required]
        [Email]
        public string  Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
