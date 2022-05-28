using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class ResendConfirmationCommandHandler : IRequestHandler<ResendConfirmationCommand, IResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMailService mailService;
        private readonly ILogger<ResendConfirmationCommandHandler> logger;

        public ResendConfirmationCommandHandler(IAccountRepository accountRepository
            ,IMailService mailService,ILogger<ResendConfirmationCommandHandler> logger)
        {
            this.accountRepository = accountRepository;
            this.mailService = mailService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = accountRepository.GetUsersByEmail(request.Email)
                    .FirstOrDefault();
                if (user == null)
                {
                    logger.LogWarning($"User with {request.Email} wasn't found");
                    return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
                }
                if(user.IsConfirmed == "true")
                {
                    return new BaseResponse(ResponseStatus.BadRequest) { Message = "Аккаунт уже подтвержден" };
                }
                await mailService.SendConfirmationAsync(user.Email, user.IsConfirmed);
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
