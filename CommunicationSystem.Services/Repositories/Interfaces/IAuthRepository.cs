using CommunicationSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task SetTimeAsync(int id, string act);
        public User GetConfirmedUser(Login user);
    }
}
