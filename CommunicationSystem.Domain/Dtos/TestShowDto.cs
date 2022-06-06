
namespace CommunicationSystem.Domain.Dtos
{
    public class TestShowDto
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public int QuestionsQuantity { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
    }
}
