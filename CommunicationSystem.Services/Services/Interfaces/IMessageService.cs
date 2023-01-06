using CommunicationSystem.Domain.Dtos;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IMessageService
    {
        public Task SendMessageAsync(SendMessageDto dto);
    }
}
