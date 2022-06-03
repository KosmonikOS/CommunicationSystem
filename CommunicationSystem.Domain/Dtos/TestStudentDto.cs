
namespace CommunicationSystem.Domain.Dtos
{
    public class TestStudentDto
    {
        public Guid TestId { get; set; }
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSelected { get; set; }
        public int? Mark { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
    }
}
