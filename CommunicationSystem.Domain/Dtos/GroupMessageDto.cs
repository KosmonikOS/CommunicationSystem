
using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class GroupMessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
        public string NickName { get; set; }
        public string AccountImage { get; set; }
        public bool IsMine { get; set; }
    }
}
