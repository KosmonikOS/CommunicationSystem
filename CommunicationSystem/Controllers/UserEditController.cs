using CommunicationSystem.Models;
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
    public class UserEditController : ControllerBase
    {
        private CommunicationContext db;
        public UserEditController(CommunicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public List<User> Get()
        {
            return (from u in db.Users
                    join r in db.Roles on u.Role equals r.Id
                    orderby u.Role descending
                    select new User() { 
                        Id = u.Id,
                        Role = u.Role,
                        RoleName = r.Name,
                        FirstName = u.FirstName,
                        accountImage = u.accountImage,
                        Email = u.Email,
                        Grade = u.Grade,
                        GradeLetter = u.GradeLetter,
                        IsConfirmed = u.IsConfirmed,
                        LastName = u.LastName,
                        MiddleName = u.MiddleName,
                        NickName = u.NickName,
                        Password = u.Password,
                        Phone = u.Phone
                    }
                ).ToList();
        }
        [HttpPost]
        public IActionResult Post(User user)
        {
            if(user != null)
            {
                try
                {
                    if (user.Id != 0)
                    {
                        db.Users.Update(user);
                    }
                    else
                    {
                        user.IsConfirmed = "true";
                        db.Users.Add(user);
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
                    var user = db.Users.SingleOrDefault(u => u.Id == id);
                    db.Users.Remove(user);
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
