using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IAccountRepository :IBaseRepository
    {
        public User GetUserByEmail(string email);
        public EntityEntry<User> AddUser(RegistrationDto user,UserSaltPass hash, string token);
        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email);
        public IResponse UpdateImage(int id, string path);
        public EntityEntry<User> UpdateUser(User user);
    }
}
