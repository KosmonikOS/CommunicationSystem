using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{email}")]
        public async Task<ActionResult<UserAccountDto>> GetUser(string email)
        {
            var query = new GetUserAccountQuery() { Email = email };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateUserImage(IFormFile imageToSave, int id)
        {
            var command = new UpdateUserImageCommand()
            {
                Id = id,
                ImageToSave = imageToSave
            };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserAccountUpdateDto dto)
        {
            var command = new UpdateUserCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
