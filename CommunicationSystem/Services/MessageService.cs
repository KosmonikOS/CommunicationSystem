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
        private readonly IClientsOfHub clients;

        public MessageService(IHubContext<MessengerHub> hubcontext,CommunicationContext db,IClientsOfHub clients)
        {
            this.hubContext = hubcontext;
            this.db = db;
            this.clients = clients;
        }
        public async Task SendMessage(Message message)
        {
            if (message.ToGroup != 0)
            {
                var members = clients.GetMessengerClients(message);
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
