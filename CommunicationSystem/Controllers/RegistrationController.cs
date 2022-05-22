using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Services.Interfaces;
using CommunicationSystem.Services.Repositories.Interfaces;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMailSender mail;
        private readonly IConfirmationToken tokenService;
        private readonly IRegistration registration;
        private readonly IAccountRepository repository;

        public RegistrationController(IMailSender mail,IConfirmationToken tokenService,IRegistration registration,IAccountRepository repository)
        {
            this.mail = mail;
            this.tokenService = tokenService;
            this.registration = registration;
            this.repository = repository;
        }
        [HttpGet("{token}")]
        public async Task<IActionResult> ConfirmUserByToken(string token)
        {
            await tokenService.ConfirmTokenAsync(token);
            return Redirect($"{this.Request.Scheme}://{this.Request.Host.Value}");
        }
        [HttpPost]
        public async Task<IActionResult> RegistrateUser(Registration user)
        {
            if(user != null)
            {
                try
                {
                    var isUnique = registration.IsUniqueEmail(user.Email);
                    if (isUnique) {
                        var token = tokenService.GenerateToken(user.Email);
                        await repository.AddUserAsync(user, token);
                        await mail.SendRegistrationmailAsync(user.Email, token, $"{this.Request.Scheme}://{this.Request.Host.Value}");
                        return Ok();
                    }
                    return BadRequest("Почта уже используется");
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
    }
}
