using CommunicationSystem.Domain.Entities;
using System.Security.Claims;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IJwtService
    {
        public List<Claim> GenerateClaims(User user);
        public Task<string> GenerateRTAsync(string email);
        public string GenerateJWT(List<Claim> claims);
        public Task<object> RefreshAsync(TokenPair pair);
        public ClaimsPrincipal GetClaims(string token);
    }
}
