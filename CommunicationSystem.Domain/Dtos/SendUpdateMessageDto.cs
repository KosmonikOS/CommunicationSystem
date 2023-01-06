using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class SendUpdateMessageDto
    {
        public int Id { get; set; }
        public bool IsGroup { get; set; }
        public string Content { get; set; }
        public int From { get; set; }
        public int? To { get; set; }
        public Guid? ToGroup { get; set; }
        public string PreviousMessage { get; set; }
        public DateTime? PreviousDate { get; set; }
        public MessageType PreviousType { get; set; }
    }
}
