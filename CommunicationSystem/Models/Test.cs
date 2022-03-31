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
        [NotMapped]
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public int Questions { get; set; }
        public long Time {get;set;}
        public DateTime Date { get; set; }
        public int Creator { get; set; }
        [NotMapped]
        public string CreatorName { get; set; }
        [NotMapped]
        public List<UsersToTests> Students { get; set; } = new List<UsersToTests>();
        [NotMapped]
        public List<Question> QuestionsList { get; set; } = new List<Question>();
    }
}
