using CommunicationSystem.Models;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public User GetUserByEmail(string email);
        public Task UpdateImage(int id, string path);
        public Task UpdateUser(User user);
    }
}
