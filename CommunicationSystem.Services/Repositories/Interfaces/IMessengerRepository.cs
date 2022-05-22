using CommunicationSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IMessengerRepository
    {
        public Task<IEnumerable<UserLastMessage>> GetContactsListAsync(int id);
        public Task<IEnumerable<UserLastMessage>> GetContactsListByNickNameAsync(int id,string nickName);
        public Task SetViewedStatusAsync(int accountId, int userId, int toGroup);
        public Task<IEnumerable<MessageBewteenUsers>> GetContactMessagesAsync(int accountId, int userId, int toGroup);
        public Task SaveMessageAsync(Message message);
        public Task SaveFileMessageAsync(IFormCollection data, int length);
        public Task DeleteMessageAsync(int id,string email);
    }
}
