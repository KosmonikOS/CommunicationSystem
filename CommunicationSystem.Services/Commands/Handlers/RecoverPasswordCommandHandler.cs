using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, IResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMailService mailService;
        private readonly IPasswordHashService hashService;
        private readonly ILogger<RecoverPasswordCommandHandler> logger;

        public RecoverPasswordCommandHandler(IAccountRepository accountRepository,IMailService mailService
            , IPasswordHashService hashService, ILogger<RecoverPasswordCommandHandler> logger)
        {
            this.accountRepository = accountRepository;
            this.mailService = mailService;
            this.hashService = hashService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(RecoverPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newPass = Guid.NewGuid().ToString();
                var saltPass = hashService.GenerateSaltPass(newPass);
                var setPass = accountRepository.UpdateUserPasswordByEmail(saltPass,request.Dto.Email);
                if (!setPass.IsSuccess) return setPass;
                await accountRepository.SaveChangesAsync();
                await mailService.SendRecoveredPasswordAsync(request.Dto.Email, newPass);
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError) { Message = e.Message };
            }
        }
    }
}
