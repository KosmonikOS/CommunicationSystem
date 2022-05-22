using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TestsController : ControllerBase
    {
        private readonly ITestRepository repository;

        public TestsController(ITestRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("{id}")]
        public async Task<List<Test>> Get(int id)
        {
            return await repository.GetUserTestsAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> Post(TestAnswer testAnswer)
        {
            try
            {
                await repository.SaveTestAnswerAsync(testAnswer);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
