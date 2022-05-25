using CommunicationSystem.Services.Infrastructure.Responses;
namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IConfirmationTokenService
    {
        public Task<IResponse> ConfirmTokenAsync(string token);
        public string GenerateToken(string email);
    }
}