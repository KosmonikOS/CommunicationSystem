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
    public class MessengerController : ControllerBase
    {
        private readonly CommunicationContext db;
        public MessengerController(CommunicationContext context)
        {
            db = context;
        }
        [HttpGet("{id}/{nickName?}")]
        public IEnumerable<UserLastMessage> Get(int id,string nickName)
        {
            if (nickName == null)
            {
                return (from u in db.Users
                        from m in db.Messages
                        where (((m.From == id && m.To == u.Id) || (m.From == u.Id && m.To == id)))
                        && m.Date == db.Messages.Where(m => ((m.From == id && m.To == u.Id) || (m.From == u.Id && m.To == id))).Max(m => m.Date)
                        select new UserLastMessage()
                        {
                            Id = u.Id,
                            accountImage = u.accountImage,
                            NickName = u.NickName,
                            MessageId = m.Id,
                            From = m.From,
                            To = m.To,
                            Content = m.Content,
                            Date = m.Date
                        }).ToList();
            }
            return (from u in db.Users
                    join m1 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = id, From = u.Id } equals new { m1.To, m1.From } into m1g
                    from g1 in m1g.DefaultIfEmpty()
                    join m2 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = u.Id, From = id } equals new { m2.To, m2.From } into m2g
                    from g2 in m2g.DefaultIfEmpty()
                    where u.Id != id && u.NickName == nickName
                    select new UserLastMessage()
                    {
                        Id = u.Id,
                        accountImage = u.accountImage,
                        NickName = u.NickName,
                        MessageId = (g1 == null ? g2 == null ? 0 : g2.Id : g1.Id),
                        From = (g1 == null ? g2 == null ? 0 : g2.From : g1.From),
                        To = (g1 == null ? g2 == null ? 0 : g2.To : g1.To),
                        Content = (g1 == null ? g2 == null ? "" : g2.Content : g1.Content),
                        Date = (g1 == null ? g2 == null ? null : g2.Date : g1.Date)
                    }).ToList();
        }
        [HttpGet("getmessages/{accountid}/{userid}")]
        public IEnumerable<MessageBewteenUsers> Get(int accountid,int userid)
        {
            return (from m in db.Messages
                    join u in db.Users on new { m.From, m.To } equals new { From = u.Id, To = accountid } into mug
                    from g in mug.DefaultIfEmpty()
                    where (m.From == accountid && m.To == userid) || (m.From == userid && m.To == accountid)
                    select new MessageBewteenUsers()
                    {
                        Id = m.Id,
                        From = m.From,
                        To = m.To,
                        Content = m.Content,
                        ToGroup = m.ToGroup,
                        Date = m.Date,
                        NickName = g == null ? "" : g.NickName,
                        AccountImage = g == null ? "" : g.accountImage
                    }
                    ).ToList();
        }
        [HttpPost]
        public IActionResult Post(Message message)
        {
            if(message != null)
            {
                try
                {
                    message.Date = DateTime.Now;
                    db.Messages.Add(message);
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
