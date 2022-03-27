using CommunicationSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IClientsOfHub
    {
        public List<ClientOfHub> GetMessengerClients(Message message);
        public List<ClientOfHub> GetVideoChatClients(UserLastMessage calling);
        public List<ClientOfHub> GetVideoChatClientsWithOutMyself(string caller, UserLastMessage calling);
        public string GetCallerNickName(string caller);
        public string GetGroupName(int id);
    }
}
