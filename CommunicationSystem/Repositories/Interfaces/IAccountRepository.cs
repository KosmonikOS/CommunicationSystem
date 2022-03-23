using CommunicationSystem.Models;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public User GetUserByEmail(string email);
        public Task UpdateImageAsync(int id, string path);
        public Task UpdateUserAsync(User user);
    }
}
