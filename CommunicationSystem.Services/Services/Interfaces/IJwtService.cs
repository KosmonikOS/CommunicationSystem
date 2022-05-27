
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using System.Security.Claims;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IJwtService
    {
        public List<Claim> GenerateClaims(User user);
        public Task<IContentResponse<string>> GenerateRTAsync(int id);
        public string GenerateJWT(List<Claim> claims);
        public Task<IContentResponse<RefreshTokenDto>> RefreshAsync(RefreshTokenDto pair);
        public IContentResponse<ClaimsPrincipal> GetClaims(string token);
    }
}
