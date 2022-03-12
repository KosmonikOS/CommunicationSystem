using CommunicationSystem.Models;
using CommunicationSystem.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly IWebHostEnvironment env;
        private readonly PathOptions options;

        public AccountController(CommunicationContext context, IWebHostEnvironment environment,IOptions<PathOptions> options)
        {
            env = environment;
            this.options = options.Value;
            db = context;
        }
        [HttpGet("{email}")]
        public User Get(string email)
        {
            return db.Users.SingleOrDefault(u => u.Email == email);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(IFormFile imageToSave, int id)
        {
            if (imageToSave != null)
            {
                try
                {
                    var path = options.AssetsFolder + DateTime.Now.TimeOfDay.TotalMilliseconds + imageToSave.FileName.ToString();
                    using (var filestr = new FileStream(Path.Combine(env.ContentRootPath + options.AssetsPath + path), FileMode.Create))
                    {
                        await imageToSave.CopyToAsync(filestr);
                    }
                    var user = db.Users.SingleOrDefault(u => u.Id == id);
                    user.accountImage = path;
                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                    return Ok(new {path =  path });
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Post(User user)
        {
            if(user != null)
            {
                try
                {
                    db.Users.Update(user);
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
