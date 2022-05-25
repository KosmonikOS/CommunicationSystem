
namespace CommunicationSystem.Domain.Dtos
{
    public class AccessTokenDto
    {
        public string AssessToken { get; set; }
        public string RefreshToken { get; set; }
        public string CurrentAccountId { get; set; }
    }
}
