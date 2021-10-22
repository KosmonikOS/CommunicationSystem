using CommunicationSystem.Hubs;
using CommunicationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
    public class MessengerController : ControllerBase
    {
        private readonly CommunicationContext db;
        private readonly IHubContext<MessengerHub> hubContext;
        private readonly IWebHostEnvironment env;
        public MessengerController(CommunicationContext context, IHubContext<MessengerHub> hubcontext, IWebHostEnvironment environment)
        {
            db = context;
            hubContext = hubcontext;
            env = environment;
        }
        [HttpGet("{id}/{nickName?}")]
        public IEnumerable<UserLastMessage> Get(int id, string nickName)
        {
            if (nickName == null)
            {
                return (from u in db.Users
                        from m in db.Messages
                        where (((m.From == id && m.To == u.Id) || (m.From == u.Id && m.To == id)))
                        && m.Date == db.Messages.Where(m => ((m.From == id && m.To == u.Id) || (m.From == u.Id && m.To == id))).Max(m => m.Date)
                        orderby m.Date, m.Id
                        select new UserLastMessage()
                        {
                            Id = u.Id,
                            accountImage = u.accountImage,
                            NickName = u.NickName,
                            Email = u.Email,
                            MessageId = m.Id,
                            From = m.From,
                            To = m.To,
                            Content = m.Type == MessageTypes.Image ? "Изображение": m.Content,
                            Date = m.Date,
                            NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                        }).ToList();
            }
            return (from u in db.Users
                    join m1 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = id, From = u.Id } equals new { m1.To, m1.From } into m1g
                    from g1 in m1g.DefaultIfEmpty()
                    join m2 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = u.Id, From = id } equals new { m2.To, m2.From } into m2g
                    from g2 in m2g.DefaultIfEmpty()
                    where u.Id != id && u.NickName == nickName
                    orderby g1.Date, g1.Id
                    select new UserLastMessage()
                    {
                        Id = u.Id,
                        accountImage = u.accountImage,
                        NickName = u.NickName,
                        Email = u.Email,
                        MessageId = (g1 == null ? g2 == null ? 0 : g2.Id : g1.Id),
                        From = (g1 == null ? g2 == null ? 0 : g2.From : g1.From),
                        To = (g1 == null ? g2 == null ? 0 : g2.To : g1.To),
                        Content = (g1 == null ? g2 == null ? "" : g2.Content : g1.Content),
                        Date = (g1 == null ? g2 == null ? null : g2.Date : g1.Date),
                        NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                    }).ToList();
        }
        [HttpGet("getmessages/{accountid}/{userid}")]
        public IEnumerable<MessageBewteenUsers> Get(int accountid, int userid)
        {
            foreach (var message in db.Messages.Where(m => m.ViewStatus == ViewStatus.isntViewed && m.From == userid && m.To == accountid))
            {
                message.ViewStatus = ViewStatus.isViewed;
                db.Messages.Update(message);
            }
            db.SaveChanges();
            return (from m in db.Messages
                    join u in db.Users on new { m.From, m.To } equals new { From = u.Id, To = accountid } into mug
                    from g in mug.DefaultIfEmpty()
                    where (m.From == accountid && m.To == userid) || (m.From == userid && m.To == accountid)
                    orderby m.Date, m.Id
                    select new MessageBewteenUsers()
                    {
                        Id = m.Id,
                        From = m.From,
                        To = m.To,
                        Content = m.Content,
                        ToGroup = m.ToGroup,
                        Date = m.Date,
                        Type = m.Type,
                        NickName = g == null ? "" : g.NickName,
                        AccountImage = g == null ? "" : g.accountImage
                    }
                    ).ToList();
        }
        [HttpPost]
        public async Task<IActionResult> Post(Message message)
        {
            if (message != null && message.To != 0 && message.Content != "")
            {
                try
                {
                    message.Date = DateTime.Now;
                    db.Messages.Add(message);
                    db.SaveChanges();
                    await hubContext.Clients.User(message.ToEmail).SendAsync("Recive", message);
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
        public async Task<IActionResult> Post(IFormCollection data,int length)
        {
            var message = new Message() {To = Int32.Parse(data["to"]),From = Int32.Parse(data["from"]), ToEmail = data["toEmail"].ToString(), ToGroup = Boolean.Parse(data["toGroup"].ToString()),Type = MessageTypes.Image };
            var files = data.Files;
            if (message != null && message.To != 0 && files != null)
            {
                try
                {
                    foreach (var file in files)
                    {
                        var path = "/assets/" + DateTime.Now.TimeOfDay.TotalMilliseconds + file.FileName.ToString();
                        message.Content = path;
                        message.Date = DateTime.Now;
                        db.Messages.Add(message);
                        using (var filestr = new FileStream(env.ContentRootPath + "/ClientApp/src" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(filestr);
                        }
                        db.SaveChanges();
                        message.Id = 0;
                    }
                    await hubContext.Clients.User(message.ToEmail).SendAsync("Recive", message);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost("groups/")]
        public IActionResult Post(Group group)
        {
            if(group != null)
            {
                try
                {
                    foreach(var user in group.Users)
                    {
                        db.UsersToGroups.Add(new UsersToGroups() { Id = user.Id, GroupId = group.Id });
                    }
                    db.Groups.Add(group);
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
        [HttpPut]
        public async Task<IActionResult> Put(IFormFile GroupImage)
        {
            if (GroupImage != null)
            {
                try
                {
                    var path = "/assets/" + DateTime.Now.TimeOfDay.TotalMilliseconds + GroupImage.FileName.ToString();
                    using (var filestr = new FileStream(env.ContentRootPath + "/ClientApp/src" + path, FileMode.Create))
                    {
                        await GroupImage.CopyToAsync(filestr);
                    }
                    return Ok(new { path = path });
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
