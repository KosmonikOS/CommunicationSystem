using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Hubs;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<MessengerHub> hubContext;
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepository userRepository;

        public MessageService(IHubContext<MessengerHub> hubContext
            , IGroupRepository groupRepository,IUserRepository userRepository)
        {
            this.hubContext = hubContext;
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }
        public async Task SendMessageAsync(SendMessageDto dto)
        {
            if (!dto.IsGroup)
            {
                await hubContext.Clients.User(dto.To.ToString())
                    .SendAsync("ReceiveMessage", dto);
            }
            else
            {
                var members = await groupRepository.GetGroupMembers((Guid)dto.ToGroup)
                    .Where(x => x.UserId != dto.From).ToListAsync();
                var sender = userRepository.GetUsers()
                    .Select(x => new{ x.Id,x.NickName,x.AccountImage })
                    .FirstOrDefault(x => x.Id == dto.From);
                foreach (var member in members)
                    await hubContext.Clients.User(member.UserId.ToString())
                        .SendAsync("ReceiveMessage", dto, sender);
            }
        }
    }
}
