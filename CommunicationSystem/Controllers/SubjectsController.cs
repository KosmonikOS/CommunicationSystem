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
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository repository;

        public SubjectsController(ISubjectRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<List<Subject>> GetSubjects()
        {
            return await repository.GetSubjectsAsync();
        }
        [HttpPost]
        public async Task<IActionResult> SaveSubject(Subject subject)
        {
            try
            {
                await repository.SaveSubjectAsync(subject);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await repository.DeleteSubjectAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
