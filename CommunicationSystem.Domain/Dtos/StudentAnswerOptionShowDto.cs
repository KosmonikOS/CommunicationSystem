
namespace CommunicationSystem.Domain.Dtos
{
    public class StudentAnswerOptionShowDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsRightOption { get; set; }
        public bool IsSelected { get; set; }
    }
}
