
namespace CommunicationSystem.Domain.Dtos
{
    public class AddFileMessageDto
    {
        public bool IsGroup { get; set; }
        public int From { get; set; }
        public int? To { get; set; }
        public Guid? ToGroup { get; set; }
        public string Content { get; set; }
        public string FileType { get; set; }

    }
}
