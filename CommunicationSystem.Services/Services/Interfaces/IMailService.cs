namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IMailService
    {
        public Task SendConfirmationAsync(string email, string token);
        public Task SendRecoveredPasswordAsync(string email, string password);
    }
}
