
namespace CommunicationSystem.Domain.Dtos
{
    public class StudentFullTestAnswerDto
    {
        public int UserId { get; set; }
        public Guid TestId { get; set; }
        public IEnumerable<StudentQuestionAnswerDto> Questions { get; set; }
    }
}
