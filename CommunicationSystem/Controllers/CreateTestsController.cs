using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreateTestsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CreateTestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("tests/{userId}/{role}/{page}/{searchOption}/{search?}")]
        public async Task<ActionResult<List<CreateTestShowDto>>> GetTestsPage(int userId, int role, int page,
            TestPageSearchOption searchOption, string search, CancellationToken cancellationToken)
        {
            var query = new GetCreateTestsQuery()
            {
                UserId = userId,
                Role = role,
                Page = page,
                Search = search,
                SearchOption = searchOption
            };
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("questions/{testId}")]
        public async Task<ActionResult<List<CreateQuestionDto>>> GetQuestionsWithOptions(Guid testId, CancellationToken cancellationToken)
        {
            var query = new GetCreateQuestionsWithOptionsQuery() { TestId = testId };
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("students/{searchOption}/{search?}")]
        public async Task<ActionResult<List<SearchStudentDto>>> GetStudents(UserSearchOption searchOption,string search
            , CancellationToken cancellationToken)
        {
            var query = new GetStudentsWithSearchQuery() { Search = search,SearchOption = searchOption }; ;
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("students/{testId}")]
        public async Task<ActionResult<List<TestStudentDto>>> GetStudents(Guid testId, CancellationToken cancellationToken)
        {
            var query = new GetTestStudentsQuery() { TestId = testId };
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("answers/{userId}/{testId}")]
        public async Task<ActionResult<List<StudentAnswerShowDto>>> GetStudentAnswers(int userid, Guid testId, CancellationToken cancellationToken)
        {
            var query = new GetStudentAnswersQuery() { UserId = userid,TestId = testId };
            var result = await mediator.Send(query,cancellationToken);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> AddTest(CreateTestDto dto)
        {
            var command = new AddTestCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTest(CreateTestDto dto)
        {
            var command = new UpdateTestCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut("mark")]
        public async Task<IActionResult> UpdateStudentMark(UpdateStudentMarkDto dto)
        {
            var command = new UpdateStudentMarkCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("test/{id}")]
        public async Task<IActionResult> DeleteTest(Guid id)
        {
            var command = new DeleteTestCommand() { TestId = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("question/{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var command = new DeleteQuestionCommand() { QuestionId = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("option/{id}")]
        public async Task<IActionResult> DeleteOption(Guid id)
        {
            var command = new DeleteOptionCommand() { OptionId = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
