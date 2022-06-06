using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("tests/{userId}/{page}/{searchOption}/{search?}")]
        public async Task<ActionResult<List<TestShowDto>>> GetTestsPage(int userId, int page, TestSearchOption searchOption, string search)
        {
            var query = new GetTestsQuery()
            {
                UserId = userId,
                Page = page,
                Searh = search,
                SearchOption = searchOption 
            };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("questions/{testId}")]
        public async Task<ActionResult<List<QuestionShowDto>>> GetQuestionsWithOptions(Guid testId)
        {
            var query = new GetQuestionsWithOptionsQuery() { TestId = testId };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> AddAnswersWithCheck(StudentFullTestAnswerDto dto)
        {
            var command = new AddStudentAnswersWithCheckCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
