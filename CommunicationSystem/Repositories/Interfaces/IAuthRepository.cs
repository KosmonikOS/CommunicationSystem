using CommunicationSystem.Models;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task SetTimeAsync(int id, string act);
        public User GetConfirmedUser(Login user);
    }
}
