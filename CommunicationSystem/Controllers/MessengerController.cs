using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMessengerRepository messengerRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IFileSaver fileService;

        public MessengerController(IMessengerRepository messengerRepository, IGroupRepository groupRepository,IFileSaver fileService)
        {
            this.messengerRepository = messengerRepository;
            this.groupRepository = groupRepository;
            this.fileService = fileService;
        }
        [HttpGet("{id}/{nickName?}")]
        public async Task<ActionResult<IEnumerable<UserLastMessage>>> GetContactsList(int id, string nickName)
        {
            try
            {
                if (nickName != null)
                {
                    return Ok(await messengerRepository.GetContactsListByNickNameAsync(id, nickName));
                }
                return Ok(await messengerRepository.GetContactsListAsync(id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("getmessages/{accountid}/{userid}/{togroup}")]
        public async Task<IEnumerable<MessageBewteenUsers>> GetContactMessages(int accountid, int userid, int togroup)
        {
            await messengerRepository.SetViewedStatusAsync(accountid, userid, togroup);
            var messages = await messengerRepository.GetContactMessagesAsync(accountid, userid, togroup);
            return messages;
        }
        [HttpPost]
        public async Task<IActionResult> SaveMessage(Message message)
        {
            if (message != null && message.Content != "")
            {
                try
                {
                    await messengerRepository.SaveMessageAsync(message);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost("filemessage/{length}")]
        public async Task<IActionResult> SaveFileMessage(IFormCollection data, int length)
        {
            try
            {
                await messengerRepository.SaveFileMessageAsync(data, length);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}/{email}")]
        public async Task<IActionResult> DeleteMessage(int id, string email)
        {
            try
            {
                await messengerRepository.DeleteMessageAsync(id, email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("groups/{id}")]
        public ActionResult<Group> GetGroup(int id)
        {
            try
            {
                var group = groupRepository.GetGroup(id);
                return Ok(group);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("groups/")]
        public async Task<IActionResult> SaveGroup(Group group)
        {
            try
            {
                await groupRepository.SaveGroupAsync(group);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGroupImage(IFormFile GroupImage)
        {
            try
            {
                var path = await fileService.SaveFileAsync(GroupImage);
                return Ok(new { path = path });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
