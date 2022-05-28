using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CommunicationContext context;
        private readonly ILogger<AccountRepository> logger;

        public AccountRepository(CommunicationContext context, ILogger<AccountRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public EntityEntry<User> AddUser(RegistrationDto user, UserSaltPass saltPass, string token)
        {
            return context.Add(new User()
            {
                Email = user.Email,
                NickName = user.NickName,
                PassHash = saltPass,
                IsConfirmed = token,
            });
        }

        public IQueryable<User> GetUsersByEmail(string email)
        {
            return context.Users.AsNoTracking()
                .Where(x => x.Email == email);
        }
        public IResponse UpdateImage(int id, string path)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                logger.LogWarning($"User with {id} id wasn't fount while updating image");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            }
            user.AccountImage = path;
            return new BaseResponse(ResponseStatus.Ok);

        }

        public EntityEntry<User> UpdateUser(User user)
        {
            var userEntry = context.Update(user);
            userEntry.Property(x => x.EnterTime).IsModified = false;
            userEntry.Property(x => x.LeaveTime).IsModified = false;
            userEntry.Property(x => x.RoleId).IsModified = false;
            userEntry.Property(x => x.IsConfirmed).IsModified = false;
            userEntry.Property(x => x.RefreshToken).IsModified = false;

            return userEntry;
        }

        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email)
        {

            var userHash = context.UserSaltPass
                .FirstOrDefault(x => x.User.Email == email);
            if (userHash == null)
            {
                logger.LogWarning($"User with {email} wasn't found");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            }
            userHash.Salt = hash.Salt;
            userHash.PasswordHash = hash.PasswordHash;
            logger.LogInformation($"User with {email} updates password");
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
