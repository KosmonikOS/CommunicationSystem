using CommunicationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationSystem.Services;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using CommunicationSystem.Services.Interfaces;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly IWebHostEnvironment env;
        private readonly IMailSender mail;

        public RegistrationController(CommunicationContext context, IWebHostEnvironment environment,IMailSender mail)
        {
            db = context;
            env = environment;
            this.mail = mail;
        }
        [HttpGet("{token}")]
        public IActionResult Get(string token)
        {
            var user = db.Users.SingleOrDefault(u => u.IsConfirmed == token);
            if(user != null)
            {
                if (Convert.ToDateTime(Encoding.UTF8.GetString(Convert.FromBase64String(token.Split("@d@")[1]))).AddSeconds(3600) >= DateTime.Now)
                {
                    user.IsConfirmed = "true";
                    db.Users.Update(user);
                }
                else
                {
                    db.Users.Remove(user);
                }
                db.SaveChanges();
            }
            return Redirect($"{this.Request.Scheme}://{this.Request.Host.Value}");
        }
        [HttpPost]
        public async Task<IActionResult> Post(Registration user)
        {
            if(user != null)
            {
                try
                {
                    var email = db.Users.SingleOrDefault(u => u.Email == user.Email);
                    if (email == null) {
                        var token = Convert.ToBase64String(Encoding.ASCII.GetBytes(user.Email)) + "@d@" + Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
                        db.Users.Add(new User() { Email = user.Email, NickName = user.NickName, Password = user.Password, IsConfirmed = token });
                        db.SaveChanges();
                        await mail.SendRegistrationmail(user.Email, token, $"{this.Request.Scheme}://{this.Request.Host.Value}");
                        return Ok();
                    }
                    return BadRequest("Почта уже используется");
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
