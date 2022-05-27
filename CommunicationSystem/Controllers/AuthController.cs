using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPut("settime")]
        public async Task<IActionResult> SetTime(UserActivityDto dto)
        {
            var command = new SetUserActivityTimeCommand()
            {
                Dto = dto
            };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<TokenPairDto>> Enter(LoginDto dto)
        {
            var command = new GenerateEnterTokenCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenDto>> Refresh(RefreshTokenDto dto)
        {
            var command = new RefreshTokenCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut("recover")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDto dto)
        {
            var command = new RecoverPasswordCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
