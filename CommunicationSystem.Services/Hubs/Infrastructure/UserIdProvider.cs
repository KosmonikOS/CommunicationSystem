using Microsoft.AspNetCore.SignalR;

namespace CommunicationSystem.Services.Hubs.Infrastructure
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
