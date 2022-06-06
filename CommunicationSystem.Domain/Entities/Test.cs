using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Grade { get; set; }
        public int QuestionsQuantity { get; private set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public int Time { get; set; }
        public DateTime Date { get; set; }

        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<User> Students { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }

        public void CalculateQuestionsQuantity()
        {
            this.QuestionsQuantity = this.Questions.Count;
        }
    }
}
