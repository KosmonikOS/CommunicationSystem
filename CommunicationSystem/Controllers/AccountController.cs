using CommunicationSystem.Services.Interfaces;
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
using CommunicationSystem.Repositories.Interfaces;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IFileSaver file;
        private readonly IAccountRepository repository;

        public AccountController(IFileSaver file, IAccountRepository repository)
        {
            this.file = file;
            this.repository = repository;
        }
        [HttpGet("{email}")]
        public ActionResult<User> GetUser(string email)
        {
            try
            {
                var user = repository.GetUserByEmail(email);
                if (user != null)
                {
                    return Ok(user);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserImage(IFormFile imageToSave, int id)
        {
            if (imageToSave != null)
            {
                try
                {
                    var path = await file.SaveFileAsync(imageToSave);
                    if (path != null)
                    {
                        await repository.UpdateImageAsync(id, path);
                        return Ok(new { path = path });
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (user != null)
            {
                try
                {
                    await repository.UpdateUserAsync(user);
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
