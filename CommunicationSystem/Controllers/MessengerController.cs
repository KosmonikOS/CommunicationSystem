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
                        where (((m.From == id && m.To == u.Id) || (m.From == u.Id && m.To == id)) && m.ToGroup == 0)
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
                            Content = m.Type == MessageTypes.Image ? "Изображение" : m.Content,
                            Date = m.Date,
                            UserActivity = UserActivity(u.EnterTime, u.LeaveTime),
                            NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                        }).ToList().Concat((from utg in db.UsersToGroups
                                            join g in db.Groups on utg.GroupId equals g.Id
                                            join m in db.Messages on g.Id equals m.ToGroup into gGroup
                                            from gm in gGroup.DefaultIfEmpty()
                                            where (utg.UserId == id) && (g.Id == gm.ToGroup) &&
                                            (gm != null && gm.Date == db.Messages.Where(m => m.ToGroup == g.Id).Max(m => m.Date))
                                            orderby gm.Date, gm.Id
                                            select new UserLastMessage()
                                            {
                                                Id = g.Id,
                                                accountImage = g.GroupImage,
                                                NickName = g.Name,
                                                Email = "Group",
                                                MessageId = gm == null ? 0 : gm.Id,
                                                From = 0,
                                                To = 0,
                                                Content = gm == null ? "" : gm.Type == MessageTypes.Image ? "Изображение" : gm.Content,
                                                Date = gm == null ? null : gm.Date,
                                                NotViewed = db.Messages.Where(m => (m.ToGroup == g.Id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                                            }).ToList()).OrderByDescending(m => m.Date);
            }
            return (from u in db.Users
                    join m1 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = id, From = u.Id } equals new { m1.To, m1.From } into m1g
                    from g1 in m1g.DefaultIfEmpty()
                    join m2 in db.Messages.OrderByDescending(m => m.Date).Take(1) on new { To = u.Id, From = id } equals new { m2.To, m2.From } into m2g
                    from g2 in m2g.DefaultIfEmpty()
                    where u.NickName.ToLower().Contains(nickName.ToLower()) && u.Id != id && ((g1 == null && g2 == null) || (g1.ToGroup == 0 || g2.ToGroup == 0))
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
                        UserActivity = UserActivity(u.EnterTime, u.LeaveTime),
                        NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                    }).ToList().Concat((from utg in db.UsersToGroups
                                        join g in db.Groups on utg.GroupId equals g.Id
                                        join m in db.Messages on g.Id equals m.ToGroup into gGroup
                                        from gm in gGroup.DefaultIfEmpty()
                                        where (utg.UserId == id) && (g.Name.Contains(nickName)) &&
                                        (gm != null && gm.Date == db.Messages.Where(m => m.ToGroup == g.Id).Max(m => m.Date))
                                        orderby gm.Date, gm.Id
                                        select new UserLastMessage()
                                        {
                                            Id = g.Id,
                                            accountImage = g.GroupImage,
                                            NickName = g.Name,
                                            Email = "Group",
                                            MessageId = gm == null ? 0 : gm.Id,
                                            From = 0,
                                            To = 0,
                                            Content = gm == null ? "" : gm.Type == MessageTypes.Image ? "Изображение" : gm.Content,
                                            Date = gm == null ? null : gm.Date,
                                            NotViewed = db.Messages.Where(m => (m.ToGroup == g.Id && m.From != id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                                        }).ToList()).OrderByDescending(m => m.Date);
        }
        [HttpGet("getmessages/{accountid}/{userid}/{togroup}")]
        public IEnumerable<MessageBewteenUsers> Get(int accountid, int userid, int togroup)
        {
            foreach (var message in db.Messages.Where(m => m.ViewStatus == ViewStatus.isntViewed && ((m.From == userid && m.To == accountid) || m.ToGroup == togroup)))
            {
                message.ViewStatus = ViewStatus.isViewed;
                db.Messages.Update(message);
            }
            db.SaveChanges();
            var temp = (from m in db.Messages
                            //join u in db.Users on new { m.From, m.To } equals new { From = u.Id, To = accountid } into mug
                        join u in db.Users on new { m.From, Me = accountid != m.From } equals new { From = u.Id, Me = true } into mug
                        from g in mug.DefaultIfEmpty()
                        where togroup == 0 ? (((m.From == accountid && m.To == userid) || (m.From == userid && m.To == accountid)) && m.ToGroup == 0) : (m.ToGroup == togroup && m.To == 0)
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
            return temp;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Message message)
        {
            if (message != null && message.Content != "")
            {
                try
                {
                    message.Date = DateTime.Now;
                    message.To = message.ToGroup != 0 ? 0 : message.To;
                    db.Messages.Add(message);
                    db.SaveChanges();
                    await SendMessage(message);
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
        public async Task<IActionResult> Post(IFormCollection data, int length)
        {
            var message = new Message() { To = Int32.Parse(data["to"]), From = Int32.Parse(data["from"]), ToEmail = data["toEmail"].ToString(), ToGroup = Int32.Parse(data["toGroup"].ToString()), Type = MessageTypes.Image };
            var files = data.Files;
            if (message != null && files != null)
            {
                try
                {
                    foreach (var file in files)
                    {
                        var path = "/assets/" + DateTime.Now.TimeOfDay.TotalMilliseconds + file.FileName.ToString();
                        message.Content = path;
                        //message.To = message.ToGroup != 0 ? 0 : message.To;
                        message.Date = DateTime.Now;
                        db.Messages.Add(message);
                        using (var filestr = new FileStream(env.ContentRootPath + "/ClientApp/src" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(filestr);
                        }
                        db.SaveChanges();
                        message.Id = 0;
                    }
                    await SendMessage(message);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpDelete("{id}/{email}")]
        public async Task<IActionResult> Delete(int id, string email)
        {
            if (id != 0)
            {
                try
                {
                    var message = db.Messages.SingleOrDefault(m => m.Id == id);
                    message.Content = "Сообщение удалено";
                    message.Type = MessageTypes.Text;
                    message.ToEmail = email;
                    db.Messages.Update(message);
                    await db.SaveChangesAsync();
                    await SendMessage(message);
                    return Ok(new { message = message });
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        private async Task SendMessage(Message message)
        {
            if (message.ToGroup != 0)
            {
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == message.ToGroup
                               select new
                               {
                                   Email = u.Email
                               });
                foreach (var member in members)
                {
                    await hubContext.Clients.User(member.Email).SendAsync("Recive", message);
                }
            }
            else
            {
                await hubContext.Clients.User(message.ToEmail).SendAsync("Recive", message);
            }
        }
        [HttpGet("groups/{id}")]
        public Group Get(int id)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Id == id);
            var users = (from utg in db.UsersToGroups
                         join u in db.Users on utg.UserId equals u.Id
                         where utg.GroupId == id
                         select new GroupUser()
                         {
                             Id = u.Id,
                             itemName = u.NickName,
                             AccountImage = u.accountImage
                         }
                         ).ToArray();
            group.Users = users;
            return group;
        }
        [HttpPost("groups/")]
        public async Task<IActionResult> Post(Group group)
        {
            if (group != null)
            {
                try
                {
                    group.GroupImage = group.GroupImage == null ? "/assets/group.png" : group.GroupImage;
                    if (group.Id == 0)
                    {
                        await db.Groups.AddAsync(group);
                        await db.SaveChangesAsync();
                        var createMessage = new Message() { To = 0, From = 0, Content = $"Группа {group.Name} создана", Date = DateTime.Now, ToGroup = group.Id };
                        db.Messages.Add(createMessage);
                        //foreach (var user in group.users)
                        //{
                        //    await db.userstogroups.addasync(new userstogroups() { userid = user.id, groupid = group.id });
                        //}
                        await db.UsersToGroups.AddRangeAsync(group.Users.Select(u => new UsersToGroups() { UserId = u.Id, GroupId = group.Id }));
                        await db.SaveChangesAsync();
                        await SendMessage(createMessage);
                    }
                    else
                    {
                        db.Groups.Update(group);
                        db.UsersToGroups.RemoveRange(db.UsersToGroups.Where(utg => utg.GroupId == group.Id));
                        await db.SaveChangesAsync();
                        await db.UsersToGroups.AddRangeAsync(group.Users.Select(u => new UsersToGroups() { UserId = u.Id, GroupId = group.Id }));
                        await db.SaveChangesAsync();
                    }
                    return Ok();
                }
                catch (Exception e)
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
        private static string UserActivity(DateTime? enter, DateTime? leave)
        {
            if (enter != null && leave != null)
            {
                var type = leave?.Date == DateTime.Today ? "t" : leave?.Year == DateTime.Now.Year ? "M" : "d";
                var seporator = leave?.Date == DateTime.Today ? " в" : ":";
                return enter > leave ? "В сети" : $"Был(a) в сети{seporator} {leave?.ToString(type)}";
            }
            return "Не в сети";
        }
    }
}
