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
        public void AddUser(RegistrationDto user,UserSaltPass hash, string token);
        public void AddUser(User user);
        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email);
        public void UpdateUser(User user,bool updateRole);
        public Task<IResponse> DeleteUserAsync(int id);
    }
}
