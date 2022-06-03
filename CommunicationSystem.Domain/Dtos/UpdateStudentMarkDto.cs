namespace CommunicationSystem.Domain.Dtos
{
    public class UpdateStudentMarkDto
    {
        public int UserId { get; set; }
        public Guid TestId { get; set; }
        public int Mark { get; set; }
    }
}
