using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Services.Interfaces;
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
        private readonly CommunicationContext db;

        public JwtService(IOptions<AuthOptions> options,CommunicationContext db)
        {
            this.options = options.Value;
            this.db = db;
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

        public async Task<string> GenerateRTAsync(string email)
        {
            var rt = "";
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[32];
                rng.GetBytes(randomNumber);
                rt = Convert.ToBase64String(randomNumber);
            }
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            user.RefreshToken = rt;
            await db.SaveChangesAsync();
            return rt;
        }

        public List<Claim> GenerateClaims(User user)
        {
            return new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Password),
                    new Claim("role",user.Role.ToString())
                };
        }

        public async Task<object> RefreshAsync(TokenPair pair)
        {
            var principal = GetClaims(pair.JWT);
            var userRT = db.Users.FirstOrDefault(u => u.Email == principal.Identity.Name).RefreshToken;
            if (pair.RT == userRT)
            {
                var token = GenerateJWT(principal.Claims.ToList());
                var rt = await GenerateRTAsync(principal.Identity.Name);
                return new
                {
                    access_token = token,
                    refresh_token = rt
                };
            }
            return null;
        }
        public ClaimsPrincipal GetClaims(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
