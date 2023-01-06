namespace CommunicationSystem.Domain.Dtos
{
    public class TokenPairDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int CurrentAccountId { get; set; }
    }
}
