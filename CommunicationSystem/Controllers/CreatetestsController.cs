using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreatetestsController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly ICreateTestRepository repository;

        public CreatetestsController(CommunicationContext context,ICreateTestRepository repository)
        {
            db = context;
            this.repository = repository;
        }
        [HttpGet("{id}")]
        public async Task<List<Test>> GetTests(int id)
        {
            var result = await repository.GetUsersTestsAsync(id);
            return result;
        }
        [HttpGet("getusers/{param}")]
        public async Task<List<UsersToTests>> GetStudents(string param)
        {
            if (param != "")
            {
                var students = await repository.GetStudentsByParamAsync(param);
                return students;
            }
            return new List<UsersToTests>() { };
        }
        [HttpGet("getanswers/{id}/{testid}")]
        public async Task<List<Question>> GetAnswers(int id, int testid)
        {
            var answers = await repository.GetUsersAnswersAsync(id, testid);
            return answers;
        }
        [HttpPost]
        public async Task<IActionResult> SaveTest(Test test)
        {
            if (test != null)
            {
                try
                {
                    await repository.SaveTestAsync(test);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPut("{id}/{testid}/{mark}")]
        public async Task<IActionResult> UpdateMark(int id, int testid, int mark)
        {
            if (id != 0 && testid != 0 && mark != 0)
            {
                try
                {
                    await repository.UpdateStudentMarkAsync(id, testid, mark);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            if (type != null && id != 0)
            {
                try
                {
                    await repository.DeleteTestEntity(type, id);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
    }
}
