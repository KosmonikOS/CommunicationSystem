using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        [NotMapped]
        public List<Option> Options { get; set; }
        public string Text { get; set; }
    }
}
