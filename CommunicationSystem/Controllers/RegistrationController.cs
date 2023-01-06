using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator mediator;

        public RegistrationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{token}")]
        public async Task<IActionResult> ConfirmUserByToken(string token)
        {
            var command = new ConfirmUserCommand { Token = token };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("resend/{email}")]
        public async Task<IActionResult> ResendConfirmation(string email)
        {
            var command = new ResendConfirmationCommand() { Email = email };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> RegistrateUser(RegistrationDto dto)
        {
            var command = new RegistrateUserCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}