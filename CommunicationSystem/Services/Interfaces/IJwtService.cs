using CommunicationSystem.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Interfaces
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
