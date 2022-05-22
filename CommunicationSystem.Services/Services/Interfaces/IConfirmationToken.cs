using System.Threading.Tasks;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IConfirmationToken
    {
        public Task ConfirmTokenAsync(string token);
        public string GenerateToken(string email);
    }
}
