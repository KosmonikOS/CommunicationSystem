using CommunicationSystem.Domain.Dtos;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IContactRepository
    {
        public IQueryable<ContactDto> GetUserContacts(int userId);
        public ContactDto GetContact(int from);
        public ContactDto GetGroupContact(Guid fromGroup);
    }
}
