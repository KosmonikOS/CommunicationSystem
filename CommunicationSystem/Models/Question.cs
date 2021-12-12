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
        OpenWhithCheck = 2,
        Open = 3
    }
    public class Question
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        [NotMapped]
        public List<Option> Options { get; set; }
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public string Image { get; set; }
        [NotMapped]
        public List<object> StudentAnswers { get; set; } = new List<object>() { };
    }
}
