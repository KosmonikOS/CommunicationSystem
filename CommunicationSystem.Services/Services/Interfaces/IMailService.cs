using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IMailService
    {
        public Task SendConfirmationAsync(string email, string token);
    }
}
