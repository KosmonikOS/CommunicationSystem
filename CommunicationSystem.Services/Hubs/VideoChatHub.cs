using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace CommunicationSystem.Services.Hubs
{
    [Authorize]
    public class VideoChatHub : Hub
    {
        private readonly IClientsOfHub clients;

        public VideoChatHub(IClientsOfHub clients)
        {
            this.clients = clients;
        }
        public async Task StartCall(string caller, UserLastMessage calling, object data)
        {
            if (calling.Email == "Group")
            {
                var members = clients.GetVideoChatClientsWithOutMyself(caller, calling);
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
                var callerName = clients.GetGroupName(calling.Id);
                var members = clients.GetVideoChatClients(calling);
                foreach (var member in members.Where(m => m.Email != caller))
                {
                    await this.Clients.User(member.Email).SendAsync("CallRequest", new { email = caller, name = callerName, groupId = calling.Id }, members);
                }
            }
            else if (calling.Email != null)
            {
                var callerName = clients.GetCallerNickName(caller);
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
        public async Task ToggleState(string caller, UserLastMessage calling, string type,bool state)
        {
            await this.Clients.User(calling.Email).SendAsync("ToggleState", caller,type,state);
        }
    }
}
