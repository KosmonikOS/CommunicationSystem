using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;

        public GroupsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupShowDto>> GetGroup(Guid id)
        {
            var query = new GetGroupByIdQuery() { Id = id };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("members/{search}")]
        public async Task<ActionResult<List<GroupSearchMemberDto>>> GetUsers(string search)
        {
            var query = new GetGroupMemberWithSearchQuery() { Search = search };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<IActionResult> AddGroup(CreateGroupDto dto)
        {
            var command = new AddGroupCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGroup(CreateGroupDto dto)
        {
            var command = new UpdateGroupCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
