using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IMessage
    {
        public Task SendMessage(Message message);
    }
}
