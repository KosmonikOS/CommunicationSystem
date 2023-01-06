using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.Dtos
{
    public class FileMessageResponseDto
    {
        public int Id { get; set; }
        public MessageType Type { get; set; }
        public string Path { get; set; }
    }
}
