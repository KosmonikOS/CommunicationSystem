using CommunicationSystem.Services.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveFileController : ControllerBase
    {
        private readonly IMediator mediator;

        public SaveFileController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("image")]
        public async Task<ActionResult<string>> SaveImage(IFormFile file)
        {
            var command = new SaveImageCommand() { File = file };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
