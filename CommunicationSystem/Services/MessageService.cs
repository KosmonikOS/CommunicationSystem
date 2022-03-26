using CommunicationSystem.Hubs;
using CommunicationSystem.Models;
using CommunicationSystem.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;

namespace CommunicationSystem.Services
{
    public class MessageService : IMessage
    {
        private readonly IHubContext<MessengerHub> hubContext;
        private readonly CommunicationContext db;

        public MessageService(IHubContext<MessengerHub> hubcontext,CommunicationContext db)
        {
            this.hubContext = hubcontext;
            this.db = db;
        }
        public async Task SendMessage(Message message)
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
    }
}
