using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        public Group GetGroup(int id);
        public Task SaveGroupAsync(Group group);
    }
}
