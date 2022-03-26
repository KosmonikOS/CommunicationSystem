using CommunicationSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        public Group GetGroup(int id);
        public Task SaveGroupAsync(Group group);
    }
}
