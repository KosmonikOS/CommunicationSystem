using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CommunicationContext context;
        private readonly IPasswordHashService hashService;
        private readonly ILogger<AuthRepository> logger;

        public AuthRepository(CommunicationContext context, IPasswordHashService hashService
            , ILogger<AuthRepository> logger)
        {
            this.context = context;
            this.hashService = hashService;
            this.logger = logger;
        }
        public IContentResponse<User> GetConfirmedUser(LoginDto dto)
        {
            var user = context.Users.AsNoTracking().Include(x => x.PassHash)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (user == null)
            {
                logger.LogWarning($"User with {dto.Email} wasn't found");
                return new ContentResponse<User>(ResponseStatus.NotFound) { Message = "Неверные данные" };
            }
            if (!hashService.ComparePasswords(user.PassHash.PasswordHash,
                user.PassHash.Salt, dto.Password))
            {
                logger.LogWarning($"Unsuccessful attempt to log in using {dto.Email} and {dto.Password} is detected");
                return new ContentResponse<User>(ResponseStatus.BadRequest) { Message = "Неверные данные" };
            }
            return new ContentResponse<User>(ResponseStatus.Ok) { Content = user };
        }

        public EntityEntry<User> SetTime(int id, UserActivityState act)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                switch (act)
                {
                    case UserActivityState.Enter:
                        user.EnterTime = DateTime.UtcNow;
                        break;
                    case UserActivityState.Leave:
                        user.LeaveTime = DateTime.UtcNow;
                        break;
                }
            }
            return context.Update(user);

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
