using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Services.Interfaces;

namespace CommunicationSystem.Services.Services
{
    public class HubClientsService : IClientsOfHub
    {
        private readonly CommunicationContext db;

        public HubClientsService(CommunicationContext db)
        {
            this.db = db;
        }

        public string GetCallerNickName(string caller)
        {
            var nickName = db.Users.SingleOrDefault(u => u.Email == caller).NickName;
            return nickName;
        }

        public string GetGroupName(int id)
        {
            var groupName = db.Groups.SingleOrDefault(g => g.Id == id).Name;
            return groupName;
        }

        public List<ClientOfHub> GetMessengerClients(Message message)
        {
            var members = (from utg in db.UsersToGroups
                           join u in db.Users on utg.UserId equals u.Id
                           where utg.GroupId == message.ToGroup
                           select new ClientOfHub
                           {
                               Email = u.Email
                           }).ToList();
            return members;
        }

        public List<ClientOfHub> GetVideoChatClients(UserLastMessage calling)
        {
            var members = (from utg in db.UsersToGroups
                           join u in db.Users on utg.UserId equals u.Id
                           where utg.GroupId == calling.Id
                           select new ClientOfHub
                           {
                               Email = u.Email
                           }).ToList();
            return members;
        }

        public List<ClientOfHub> GetVideoChatClientsWithOutMyself(string caller, UserLastMessage calling)
        {
            var members = (from utg in db.UsersToGroups
                           join u in db.Users on utg.UserId equals u.Id
                           where utg.GroupId == calling.Id && u.Email != caller
                           select new ClientOfHub
                           {
                               Email = u.Email
                           }).ToList();
            return members;
        }
    }
}
