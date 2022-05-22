using Microsoft.AspNetCore.SignalR;

namespace CommunicationSystem.Services.Hubs
{
    public class EmailUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
