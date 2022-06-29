using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Filters;
using CommunicationSystem.Services.Commands;
using CommunicationSystem.Services.Queries;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessengerController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<MessengerController> logger;

        public MessengerController(IMediator mediator,ILogger<MessengerController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }
        [HttpGet("contact/messages/{userId}/{contactId}/{page}")]
        public async Task<ActionResult<List<ContactMessageDto>>> GetContactMessages(int userId, int contactId, int page)
        {
            var query = new GetMessagesBetweenContactsQuery() { UserId = userId, ContactId = contactId, Page = page };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("group/messages/{userId}/{groupId}/{page}")]
        public async Task<ActionResult<List<GroupMessageDto>>> GetGroupMessages(int userId, Guid groupId, int page)
        {
            var query = new GetGroupMessagesQuery { UserId = userId, GroupId = groupId, Page = page };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<int>> AddMessage(AddMessageDto dto)
        {
            var command = new AddMessageCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPost("files")]
        [DisableFormValueModelBinding]
        public async Task<ActionResult<List<int>>> AddFileMessages()
        {
            var command = new AddFileMessageCommand() { ControllerContext = this};
            var result = await mediator.Send(command);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut("view/{id}")]
        public async Task<IActionResult> ViewMessage(int id)
        {
            var command = new ViewMessageCommand() { Id = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpPut("content")]
        public async Task<IActionResult> UpdateMessageContent(MessageContentUpdateDto dto)
        {
            var command = new UpdateMessageContentCommand() { Dto = dto };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var command = new DeleteMessageCommand() { Id = id };
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
