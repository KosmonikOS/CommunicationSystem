
namespace CommunicationSystem.Domain.Dtos
{
    public class ContactSearchDto
    {
        public int ToId { get; set; }
        public string NickName { get; set; }
        public string AccountImage { get; set; }
        public string? LastActivity { get; set; }
    }
}
