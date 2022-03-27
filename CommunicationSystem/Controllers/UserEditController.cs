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
    public class UserEditController : ControllerBase
    {
        private readonly IUserEditRepository repository;

        public UserEditController(IUserEditRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await repository.GetUsersAsync();
        }
        [HttpGet("getroles")]
        public async Task<List<Role>> GetRoles()
        {
            return await repository.GetRolesAsync();
        }
        [HttpPost]
        public async Task<IActionResult> SaveUser(User user)
        {
            try
            {
                await repository.SaveUserAsync(user);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
                try
                {
                    await repository.DeleteUserAsync(id);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
        }
    }
}
