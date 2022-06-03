using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class TestStudentStateDto
    {
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }
        public StudentState State { get; set; } = StudentState.Unchanged;
    }
}
