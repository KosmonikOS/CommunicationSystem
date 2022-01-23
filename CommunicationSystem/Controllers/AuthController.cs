using CommunicationSystem.Models;
using CommunicationSystem.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly IOptions<AuthOptions> options;

        public AuthController(CommunicationContext context, IOptions<AuthOptions> options)
        {
            db = context;
            this.options = options;
        }
        [HttpGet("settime/{id}/{act?}")]
        public async Task<IActionResult> SetEnterTime(int id, string act)
        {
            if (id != 0)
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == id);
                    switch (act)
                    {
                        case "enter":
                            user.EnterTime = DateTime.Now;
                            break;
                        case "leave":
                            user.LeaveTime = DateTime.Now;
                            break;
                    }
                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Post(Login login)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == login.Password && u.IsConfirmed == "true");
            if(user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Password),
                    new Claim("role",user.Role.ToString())
                };
                var token = GenerateJWT(claims);
                var rt = GenerateRT(login.Email) ;
                return Ok(
                    new
                    {
                        access_token = token,
                        refresh_token = rt,
                        current_account_id = user.Id
                    });
            }
            return Unauthorized();
        }
        [HttpPost("refresh")]
        public IActionResult Refresh(TokenPair pair)
        {
            var principal = GetClaims(pair.JWT);
            var userRT = db.Users.FirstOrDefault(u => u.Email == principal.Identity.Name).RefreshToken;
            if (pair.RT == userRT)
            {
                var token = GenerateJWT(principal.Claims.ToList());
                var rt = GenerateRT(principal.Identity.Name);
                return Ok(
                    new
                    {
                        access_token = token,
                        refresh_token = rt,
                    });
            }
            return BadRequest();
        }
        private ClaimsPrincipal GetClaims(string token)
        {
            var AuthParams = options.Value;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthParams.GetSymmetricSecurityKey(),
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
        private string GenerateJWT(List<Claim> claims)
        {
            var AuthParams = options.Value;
            var credentials = new SigningCredentials(AuthParams.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                AuthParams.Issuer,
                AuthParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(AuthParams.TokenLifeTime),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRT(string email)
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
            db.SaveChanges();
            return rt;
        }
    }
}
