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
        public async Task StartCall(string caller,UserLastMessage calling,object data)
        {
            if (calling.Email == "Group")
            {
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == calling.Id
                               select new
                               {
                                   Email = u.Email
                               });
                foreach (var member in members)
                {
                    await this.Clients.User(member.Email).SendAsync("Accept", caller,data);
                }
            }
            else
            {
                await this.Clients.User(calling.Email).SendAsync("Accept", caller,data);
            }
        }
        public async Task ToggleState(UserLastMessage calling,bool state,string type)
        {
            if (calling.Email == "Group")
            {
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == calling.Id
                               select new
                               {
                                   Email = u.Email
                               });
                foreach (var member in members)
                {
                        await this.Clients.User(member.Email).SendAsync("Toggle" + type, state);
                }
            }
            else
            {
                await this.Clients.User(calling.Email).SendAsync("Toggle" + type, state);
            }
        }
        public async Task DestroyConnection(UserLastMessage calling)
        {
            if (calling.Email == "Group")
            {
                var members = (from utg in db.UsersToGroups
                               join u in db.Users on utg.UserId equals u.Id
                               where utg.GroupId == calling.Id
                               select new
                               {
                                   Email = u.Email
                               });
                foreach (var member in members)
                {
                    await this.Clients.User(member.Email).SendAsync("Toggle");
                }
            }
            else if(calling.Email != null)
            {
                await this.Clients.User(calling.Email).SendAsync("DestroyConnection");
            }
        }
    }
}
