using CommunicationSystem.Models;
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
    public class SubjectsController : ControllerBase
    {
        private readonly CommunicationContext db;
        public SubjectsController(CommunicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public List<Subject> Get()
        {
            return db.Subjects.ToList();
        }
        [HttpPost]
        public IActionResult Post(Subject subject)
        {
            if(subject != null)
            {
                try
                {
                    if (subject.Id != 0)
                    {
                        db.Subjects.Update(subject);
                    }
                    else
                    {
                        db.Subjects.Add(subject);
                    }
                    db.SaveChanges();
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if(id != 0) 
            {
                try
                {
                    var subject = db.Subjects.SingleOrDefault(u => u.Id == id);
                    db.Subjects.Remove(subject);
                    db.SaveChanges();
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
    }
}
