using CommunicationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly CommunicationContext db;
        public RegistrationController(CommunicationContext context)
        {
            db = context;
        }
        [HttpPost]
        public IActionResult Post(User user)
        {
            if(user != null)
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
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
