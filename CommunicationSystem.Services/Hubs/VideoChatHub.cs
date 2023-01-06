using Microsoft.AspNetCore.SignalR;

namespace CommunicationSystem.Services.Hubs
{
    public class VideoChatHub :Hub
    {
        public async Task ConnectToRoom(string roomId,string peerId)
        {
            await Clients.Group(roomId).SendAsync("UserConnected", peerId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
        public async Task DisconnectFromRoom(string roomId, string peerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserDisconnected", peerId);
        }
        public async Task ToggleState(string roomId,string peerId,int type,bool value)
        {
            await Clients.Group(roomId).SendAsync("StateToggled", peerId,type,value);
        }
    }
}
