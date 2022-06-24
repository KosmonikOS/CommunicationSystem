using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
        public bool IsMine { get; set; }
    }
}
