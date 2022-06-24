using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class SendMessageDto
    { 
        public int Id { get; set; }
        public bool IsGroup { get; set; }        
        public string Content { get; set; }
        public int From { get; set; }
        public int? To { get; set; }
        public Guid? ToGroup { get; set; }
        public MessageType Type { get; set; }

    }
}
