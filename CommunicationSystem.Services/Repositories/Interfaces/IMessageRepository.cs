using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository
    {
        public IQueryable<ContactMessageDto> GetMessagesBetweenContacts(int userId, int contactId, int page);
        public IQueryable<GroupMessageDto> GetGroupMessages(int userId, Guid groupId, int page);
        public void AddMessage(Message message);
        public void UpdateMessageContent(int id,string content);
        public void DeleteMessage(int id);
        public void ViewMessage(int id);
    }
}
