using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CommunicationSystem.Services.Interfaces;
using System;
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Repositories
{
    public class MessengerRepository : IMessengerRepository
    {
        private readonly CommunicationContext db;
        private readonly IUserActivity activity;
        private readonly IMessage messageService;
        private readonly IFileSaver fileService;

        public MessengerRepository(CommunicationContext db, IUserActivity activity, IMessage messageService, IFileSaver fileService)
        {
            this.db = db;
            this.activity = activity;
            this.messageService = messageService;
            this.fileService = fileService;
        }

        public async Task DeleteMessageAsync(int id,string email)
        {
            if (id != 0)
            {
                var message = db.Messages.SingleOrDefault(m => m.Id == id);
                message.Content = "Сообщение удалено";
                message.Type = MessageTypes.Text;
                message.ToEmail = email;
                db.Messages.Update(message);
                await db.SaveChangesAsync();
                await messageService.SendMessage(message);
            }
        }

        public async Task<IEnumerable<MessageBewteenUsers>> GetContactMessagesAsync(int accountId, int userId, int toGroup)
        {
            var messages = await (from m in db.Messages
                                  join u in db.Users on new { m.From, Me = accountId != m.From } equals new { From = u.Id, Me = true } into mug
                                  from g in mug.DefaultIfEmpty()
                                  where toGroup == 0 ? (((m.From == accountId && m.To == userId) || (m.From == userId && m.To == accountId)) && m.ToGroup == 0) : (m.ToGroup == toGroup && m.To == 0)
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
                    ).ToListAsync();
            return messages;
        }

        public async Task<IEnumerable<UserLastMessage>> GetContactsListAsync(int id)
        {
            var userMessages = await (from u in db.Users
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
                                          UserActivity = activity.GetUserActivity(u.EnterTime, u.LeaveTime),
                                          NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                                      }).AsNoTracking().ToListAsync();
            var groupMessages = await (from utg in db.UsersToGroups
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
                                       }).AsNoTracking().ToListAsync();
            return userMessages.Concat(groupMessages).OrderByDescending(m => m.Date);
        }

        public async Task<IEnumerable<UserLastMessage>> GetContactsListByNickNameAsync(int id, string nickName)
        {
            var userMessages = await (from u in db.Users
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
                                          UserActivity = activity.GetUserActivity(u.EnterTime, u.LeaveTime),
                                          NotViewed = db.Messages.Where(m => (m.From == u.Id && m.To == id) && m.ViewStatus == ViewStatus.isntViewed).Count()
                                      }).ToListAsync();
            var groupMessages = await (from utg in db.UsersToGroups
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
                                       }).ToListAsync();
            return userMessages.Concat(groupMessages).OrderByDescending(m => m.Date);
        }

        public async Task SaveFileMessageAsync(IFormCollection data, int length)
        {
            var message = new Message()
            {
                To = Int32.Parse(data["to"]),
                From = Int32.Parse(data["from"]),
                ToEmail = data["toEmail"].ToString(),
                ToGroup = Int32.Parse(data["toGroup"].ToString()),
                Type = MessageTypes.Image
            };
            var files = data.Files;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var path = await fileService.SaveFileAsync(file);
                    message.Content = path;
                    message.Date = DateTime.Now;
                    db.Messages.Add(message);
                    await db.SaveChangesAsync();
                    message.Id = 0;
                    await messageService.SendMessage(message);
                }
            }
        }
        public async Task SaveMessageAsync(Message message)
        {
            message.Date = DateTime.Now;
            message.To = message.ToGroup != 0 ? 0 : message.To;
            db.Messages.Add(message);
            await db.SaveChangesAsync();
            await messageService.SendMessage(message);
        }

        public async Task SetViewedStatusAsync(int accountId, int userId, int toGroup)
        {
            foreach (var message in db.Messages.Where(m => m.ViewStatus == ViewStatus.isntViewed && ((m.From == userId && m.To == accountId) || (m.To == 0 && m.ToGroup == toGroup))))
            {
                message.ViewStatus = ViewStatus.isViewed;
                db.Messages.Update(message);
            }
            await db.SaveChangesAsync();
        }
    }
}
