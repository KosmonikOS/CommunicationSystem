using CommunicationSystem.Domain.Dtos;
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
    public class ContactsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<ContactDto>>> GetContacts(int userId)
        {
            var query = new GetUserContactsQuery() { UserId = userId };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("contact/{from}")]
        public async Task<ActionResult<ContactDto>> GetUserContact(int from)
        {
            var query = new GetContactQuery() { From = from };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("group/{fromGroup}")]
        public async Task<ActionResult<ContactDto>> GetGroupGroup(Guid fromGroup)
        {
            var query = new GetGroupContactQuery() { FromGroup = fromGroup };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
        [HttpGet("search/{search}")]
        public async Task<ActionResult<List<ContactSearchDto>>> GetContactsBySearch(string search)
        {
            var query = new GetContactsWithSearchQuery() { Search = search };
            var result = await mediator.Send(query);
            return result.IsSuccess ? Ok(result.Content) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
