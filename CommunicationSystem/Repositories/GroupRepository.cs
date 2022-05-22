using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using CommunicationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly CommunicationContext db;
        private readonly IMessage messageService;

        public GroupRepository(CommunicationContext db, IMessage messageService)
        {
            this.db = db;
            this.messageService = messageService;
        }
        public Group GetGroup(int id)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Id == id);
            if (group != null)
            {
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
            }
            return group;
        }

        public async Task SaveGroupAsync(Group group)
        {
            if (group != null)
            {
                if (group.Id == 0)
                {
                    await db.Groups.AddAsync(group);
                    await db.SaveChangesAsync();
                    var createMessage = new Message() { To = 0, From = 0, Content = $"Группа {group.Name} создана", Date = DateTime.Now, ToGroup = group.Id };
                    db.Messages.Add(createMessage);
                    await db.UsersToGroups.AddRangeAsync(group.Users.Select(u => new UsersToGroups() { UserId = u.Id, GroupId = group.Id }));
                    await db.SaveChangesAsync();
                    await messageService.SendMessage(createMessage);
                }
                else
                {
                    db.Groups.Update(group);
                    db.UsersToGroups.RemoveRange(db.UsersToGroups.AsNoTracking().Where(utg => utg.GroupId == group.Id));
                    //await db.SaveChangesAsync();
                    await db.UsersToGroups.AddRangeAsync(group.Users.Select(u => new UsersToGroups() { UserId = u.Id, GroupId = group.Id }));
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
