using CommunicationSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IUserEditRepository
    {
        public Task<List<User>> GetUsersAsync();
        public Task SaveUserAsync(User user);
        public Task DeleteUserAsync(int id);
        public Task<List<Role>> GetRolesAsync();
    }
}
