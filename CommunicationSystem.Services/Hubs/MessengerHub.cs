using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CommunicationSystem.Services.Hubs
{
    [Authorize]
    public class MessengerHub: Hub
    {
    }
}
