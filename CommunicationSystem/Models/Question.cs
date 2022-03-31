using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public enum QuestionType
    {
        Single = 0,
        Multy = 1,
        OpenWithCheck = 2,
        Open = 3
    }
    public class Question
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        [NotMapped]
        public List<Option> Options { get; set; } = new List<Option>();
        public string Text { get; set; }
        public int Points { get; set; } = 1;
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public string Image { get; set; }
        [NotMapped]
        public List<string> StudentAnswers { get; set; } = new List<string>();
        [NotMapped]
        public string OpenAnswer { get; set; }
    }
}
