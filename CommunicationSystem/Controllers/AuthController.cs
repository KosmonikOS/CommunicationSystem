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
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly IOptions<AuthOptions> options;

        public AuthController(CommunicationContext context,IOptions<AuthOptions> options)
        {
            db = context;
            this.options = options;
        }
        [HttpGet]
        public void Get()
        {
            var rnd = new Random();
        }
        [HttpPost]
        public IActionResult Post(Login login)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == login.Password && u.IsConfirmed == "true");
            if(user != null)
            {
                var token = GenerateJWT(user);
                return Ok(
                    new
                    {
                        access_token = token
                    });
            }
            return Unauthorized();
        }
        private string GenerateJWT(User user)
        {
            var AuthParams = options.Value;
            var credentials = new SigningCredentials(AuthParams.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Password),
                new Claim("role",user.Role.ToString())
            };
            var token = new JwtSecurityToken(
                AuthParams.Issure,
                AuthParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(AuthParams.TokenLifeTime),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
