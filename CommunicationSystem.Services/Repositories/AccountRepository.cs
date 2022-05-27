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

        public User GetUserByEmail(string email)
        {
            return context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email);
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
            return context.Update(user);
        }

        public IResponse UpdateUserPasswordByEmail(UserSaltPass hash, string email)
        {

            var user = context.Users.Include(x => x.PassHash)
                .FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                logger.LogWarning($"User with {email} wasn't found");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            }
            user.PassHash = hash;
            logger.LogInformation($"User with {email} recovers password");
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
