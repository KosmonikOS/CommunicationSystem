using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class Test
    {
        public long Id { get; set; }
        public int Subject { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string Name { get; set; }
        public int Grade { get; set; }
        public int Questions { get; set; }
        public long Time {get;set;}
        public DateTime Date { get; set; }
        public int Creator { get; set; }
        [NotMapped]
        public string CreatorName { get; set; }
        
    }
}
