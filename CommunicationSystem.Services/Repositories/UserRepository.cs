using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CommunicationSystem.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CommunicationContext context;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(CommunicationContext context, ILogger<UserRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public IQueryable<User> GetUsers(Expression<Func<User, bool>> expression = null)
        {
            var query = context.Users.AsNoTracking();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return query;

        }
        public IQueryable<User> GetUsersPage(int page, string search, UserSearchOption searchOption)
        {
            var query = context.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (searchOption)
                {
                    case UserSearchOption.NickName:
                        query = query.Where(x => EF.Functions.ILike(x.NickName, $"%{search}%"));
                        break;
                    case UserSearchOption.FullName:
                        query = query.Where(x => EF.Functions.ILike(
                            x.LastName + " " + x.FirstName + " " + x.MiddleName, $"%{search}%"));
                        break;
                    case UserSearchOption.Email:
                        query = query.Where(x => EF.Functions.ILike(x.Email, $"%{search}%"));
                        break;
                    case UserSearchOption.Role:
                        query = query.Include(x => x.Role)
                            .Where(x => EF.Functions.ILike(x.Role.Name, $"%{search}%"));
                        break;
                }
            }
            query = query.OrderBy(x => x.Id);
            if (page > 0)
            {
                query = query.Skip(page * 50);
            }
            return query.Take(50);
        }
        public void AddUser(RegistrationDto user, UserSaltPass saltPass, string token)
        {
            context.Add(new User()
            {
                Email = user.Email,
                NickName = user.NickName,
                PassHash = saltPass,
                IsConfirmed = token,
            });
        }
        public void AddUser(User user)
        {
            context.Add(user);
        }
        public void UpdateUser(User user, bool updateRole)
        {
            var userEntry = context.Update(user);
            userEntry.Property(x => x.RoleId).IsModified = updateRole;
            userEntry.Property(x => x.EnterTime).IsModified = false;
            userEntry.Property(x => x.LeaveTime).IsModified = false;
            userEntry.Property(x => x.IsConfirmed).IsModified = false;
            userEntry.Property(x => x.RefreshToken).IsModified = false;
        }
        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email)
        {

            var userHash = context.UserSaltPass
                .FirstOrDefault(x => x.User.Email == email);
            if (userHash == null)
            {
                logger.LogWarning($"User with {email} wasn't found");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Невозможно обновить пароль" };
            }
            userHash.Salt = hash.Salt;
            userHash.PasswordHash = hash.PasswordHash;
            logger.LogInformation($"User with {email} updates password");
            return new BaseResponse(ResponseStatus.Ok);

        }
        public async Task<IResponse> DeleteUserAsync(int id)
        {
            var user = await context.Users
                .Include(x => x.PassHash)
                .Include(x => x.Tests)
                .Include(x => x.Groups)
                .Include(x => x.StudentAnswers)
                .Include(x => x.CreatedTests)
                .ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Include(x => x.CreatedTests)
                .ThenInclude(y => y.StudentAnswers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            context.Remove(user);
            return new BaseResponse(ResponseStatus.Ok);
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
