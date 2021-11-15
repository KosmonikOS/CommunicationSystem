using CommunicationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Hubs
{
    [Authorize]
    public class VideoChatHub : Hub
    {
        private readonly CommunicationContext db;
        public VideoChatHub(CommunicationContext context)
        {
            db = context;
        }
        public async Task StartCall(string caller, UserLastMessage calling, object data)
        {
            if (calling.Email == "Group")
            {
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == calling.Id && u.Email != caller
                               select new
                               {
                                   Email = u.Email
                               });
                foreach (var member in members)
                {
                    await this.Clients.User(member.Email).SendAsync("Accept", caller, data);
                }
            }
            else
            {
                await this.Clients.User(calling.Email).SendAsync("Accept", caller, data);
            }
        }
        public async Task Ask(string caller, UserLastMessage calling)
        {
            if (calling.Email == "Group")
            {
                var callerName = db.Groups.SingleOrDefault(g => g.Id == calling.Id).Name;
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == calling.Id
                               select new
                               {
                                   Email = u.Email
                               }).ToList(); ;
                foreach (var member in members.Where(m => m.Email != caller))
                {
                    await this.Clients.User(member.Email).SendAsync("CallRequest", new { email = caller, name = callerName, groupId = calling.Id }, members);
                }
            }
            else if (calling.Email != null)
            {
                var callerName = db.Users.SingleOrDefault(u => u.Email == caller).NickName;
                await this.Clients.User(calling.Email).SendAsync("CallRequest", new { email = caller, name = callerName }, new List<object> { new { Email = caller } });
            }
        }
        public async Task React(string caller, string reaction, UserLastMessage calling)
        {
            await this.Clients.User(calling.Email).SendAsync(reaction, caller);
        }
        public async Task NeedToConnect(string calling, List<string> members)
        {
            await this.Clients.User(calling).SendAsync("OfferToConnect", members);
        }
        public async Task ToggleState(string caller, UserLastMessage calling, string type)
        {
            await this.Clients.User(calling.Email).SendAsync("ToggleState", caller,type);
        }
    }
}
