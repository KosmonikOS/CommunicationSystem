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
        private readonly IUserRepository userRepository;
        private readonly IMailService mailService;
        private readonly ILogger<ResendConfirmationCommandHandler> logger;

        public ResendConfirmationCommandHandler(IUserRepository userRepository
            , IMailService mailService, ILogger<ResendConfirmationCommandHandler> logger)
        {
            this.userRepository = userRepository;
            this.mailService = mailService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetUsers(x => x.Email == request.Email)
                .FirstOrDefault();
            if (user == null)
            {
                logger.LogWarning($"User with {request.Email} wasn't found");
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            }
            if (user.IsConfirmed == "true")
            {
                return new BaseResponse(ResponseStatus.BadRequest) { Message = "Аккаунт уже подтвержден" };
            }
            await mailService.SendConfirmationAsync(user.Email, user.IsConfirmed);
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
