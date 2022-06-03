
namespace CommunicationSystem.Domain.Dtos
{
    public class StudentAnswerOptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsRightOption { get; set; }
        public bool IsSelected { get; set; }
    }
}
