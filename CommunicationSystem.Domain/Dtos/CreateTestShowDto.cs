
namespace CommunicationSystem.Domain.Dtos
{
    public class CreateTestShowDto
    {
        public Guid Id { get; set; }
        public int Subject { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public int QuestionsQuantity { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
        public int Creator { get; set; }
        public string CreatorName { get; set; }
    }
}
