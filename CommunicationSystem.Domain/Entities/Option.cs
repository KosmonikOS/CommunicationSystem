using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsRightOption { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
