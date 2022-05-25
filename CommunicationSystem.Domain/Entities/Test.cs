
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Test
    {
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Grade { get; set; }
        public int QuestionsQuantity { get; set; }
        public long Time { get; set; }
        public DateTime Date { get; set; }

        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<User> Students { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }

    }
}
