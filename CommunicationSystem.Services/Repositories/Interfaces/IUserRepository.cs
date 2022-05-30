using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IUserRepository :IBaseRepository
    {
        public IQueryable<User> GetUsersPage(int page,string search, UserSearchOption searchOption);
        public IQueryable<User> GetUsers(Expression<Func<User, bool>> expression);
        public EntityEntry<User> AddUser(RegistrationDto user,UserSaltPass hash, string token);
        public EntityEntry<User> AddUser(User user);
        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email);
        public EntityEntry<User> UpdateUser(User user,bool updateRole);
        public Task<EntityEntry<User>> DeleteUserAsync(int id);
    }
}
