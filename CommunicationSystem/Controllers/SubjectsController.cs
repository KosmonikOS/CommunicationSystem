using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubjectsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<Subject>>> GetSubjects(CancellationToken cancellationToken)
        {
            var query = new GetSubjectsForTestQuery();
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("{page}/{search?}")]
        public async Task<ActionResult<List<Subject>>> GetSubjectsPage(int page, string search, CancellationToken cancellationToken)
        {
            var query = new GetSubjectsQuery() { Search = search, Page = page };
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectDto dto)
        {
            var command = new AddSubjectCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSubject(SubjectDto dto)
        {
            var command = new UpdateSubjectCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var command = new DeleteSubjectCommand() { Id = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
