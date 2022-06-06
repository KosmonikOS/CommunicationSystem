using CommunicationSystem.Data;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CommunicationSystem.Services.Services
{
    public class ConfirmationTokenService : IConfirmationTokenService
    {
        private readonly CommunicationContext context;
        private readonly ILogger<ConfirmationTokenService> logger;

        public ConfirmationTokenService(CommunicationContext context, ILogger<ConfirmationTokenService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IResponse> ConfirmTokenAsync(string token)
        {
            var user = context.Users.AsNoTracking().FirstOrDefault(u => u.IsConfirmed == token);
            if (user == null)
            {
                logger.LogWarning($"User with {token} token not found");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            }
            var timeStamp = Convert.ToDateTime(Encoding.UTF8.GetString(Convert.FromHexString(token.Split("@d@")[1]))).AddSeconds(3600);
            if (timeStamp < DateTime.Now)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                logger.LogWarning($"Confirmation time of {token} token is up");
                return new BaseResponse(ResponseStatus.BadRequest) { Message = "Время подтверждения истекло" };
            }
            user.IsConfirmed = "true";
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }

        public string GenerateToken(string email)
        {
            var token = Convert.ToHexString(Encoding.ASCII.GetBytes(email)) + "@d@" +
                Convert.ToHexString(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
            return token;
        }
    }
}
