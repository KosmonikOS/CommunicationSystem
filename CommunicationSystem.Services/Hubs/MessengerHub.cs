using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Hubs
{
    public class MessengerHub : Hub
    {
        private readonly IGroupRepository groupRepository;

        public MessengerHub(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }
        public async Task DeleteMessage(SendDeleteMessageDto dto)
        {
            if (!dto.IsGroup)
            {
                await Clients.User(dto.To.ToString()).SendAsync("DeleteMessage", dto);
            }
            else
            {
                var members = await groupRepository.GetGroupMembers((Guid)dto.ToGroup)
                    .Where(x => x.UserId != dto.From).ToListAsync();
                foreach (var member in members)
                    await Clients.User(member.UserId.ToString())
                        .SendAsync("DeleteMessage", dto);
            }
        }
        public async Task UpdateMessage(SendUpdateMessageDto dto)
        {
            if (!dto.IsGroup)
            {
                await Clients.User(dto.To.ToString()).SendAsync("UpdateMessage", dto);
            }
            else
            {
                var members = await groupRepository.GetGroupMembers((Guid)dto.ToGroup)
                   .Where(x => x.UserId != dto.From).ToListAsync();
                foreach (var member in members)
                    await Clients.User(member.UserId.ToString())
                        .SendAsync("UpdateMessage", dto);
            }
        }
    }
}
