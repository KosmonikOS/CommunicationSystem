using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEditController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserEditController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{page}/{searchOption}/{search?}")]
        public async Task<ActionResult<List<UserAccountAdminDto>>> GetUsersPage(int page, UserSearchOption searchOption, string search)
        {
            var query = new GetUsersQuery()
            {
                Page = page,
                Search = search,
                SearchOption = searchOption,
            };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("getroles")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var query = new GetRolesQuery();
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserAccountAdminAddDto dto)
        {
            var command = new AddUserByAdminCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserAccountAdminUpdateDto dto)
        {
            var command = new UpdateUserByAdminCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var command = new DeleteUserByAdminCommand() { Id = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
