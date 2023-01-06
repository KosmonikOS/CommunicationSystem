using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CommunicationSystem.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly AuthOptions options;
        private readonly CommunicationContext context;
        private readonly ILogger<JwtService> logger;

        public JwtService(IOptions<AuthOptions> options, CommunicationContext context
            , ILogger<JwtService> logger)
        {
            this.options = options.Value;
            this.context = context;
            this.logger = logger;
        }
        public List<Claim> GenerateClaims(User user)
        {
            return new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub,user.PassHash.PasswordHash),
                    new Claim("role",user.Role.RoleId.ToString())
                };
        }

        public string GenerateJWT(List<Claim> claims)
        {
            var credentials = new SigningCredentials(options.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                options.Issuer,
                options.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(options.TokenLifeTime),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IContentResponse<string>> GenerateRTAsync(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
                return new ContentResponse<string>(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            var rt = "";
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                var token = new byte[32];
                rngCsp.GetNonZeroBytes(token);
                rt = Convert.ToBase64String(token);
            }
            user.RefreshToken = rt;
            await context.SaveChangesAsync();
            return new ContentResponse<string>(ResponseStatus.Ok) { Content = rt };
        }

        public IContentResponse<ClaimsPrincipal> GetClaims(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.GetSymmetricSecurityKey(),
                ValidateLifetime = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                logger.LogError($"{token} token is invalid");
                return new ContentResponse<ClaimsPrincipal>(ResponseStatus.BadRequest) { Message = "Некорректные данные" };
            }
            return new ContentResponse<ClaimsPrincipal>(ResponseStatus.Ok) { Content = principal };
        }

        public async Task<IContentResponse<RefreshTokenDto>> RefreshAsync(RefreshTokenDto pair)
        {
            var principal = GetClaims(pair.AccessToken);
            if (!principal.IsSuccess)
                return new ContentResponse<RefreshTokenDto>(principal.Status) { Message = principal.Message };
            var userId = Convert.ToInt32(principal.Content.Identity.Name);
            var userRT = context.Users.Find(userId).RefreshToken;
            if (pair.RefreshToken != userRT)
            {
                logger.LogWarning($"User's RCs with {principal.Content.Identity.Name} id don't match");
                return new ContentResponse<RefreshTokenDto>(ResponseStatus.BadRequest) { Message = "Некорректные данные" };
            }
            var rt = await GenerateRTAsync(userId);
            if (!rt.IsSuccess)
                return new ContentResponse<RefreshTokenDto>(rt.Status) { Message = rt.Message };
            var token = GenerateJWT(principal.Content.Claims.ToList());
            return new ContentResponse<RefreshTokenDto>(ResponseStatus.Ok)
            {
                Content = new RefreshTokenDto()
                {
                    AccessToken = token,
                    RefreshToken = rt.Content,
                }
            };
        }
    }
}
