
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using System.Security.Claims;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IJwtService
    {
        public List<Claim> GenerateClaims(User user,string passwordHash);
        public Task<IContentResponse<string>> GenerateRTAsync(int id);
        public string GenerateJWT(List<Claim> claims);
        public Task<IContentResponse<AccessTokenDto>> RefreshAsync(TokenPairDto pair);
        public IContentResponse<ClaimsPrincipal> GetClaims(string token);
    }
}
