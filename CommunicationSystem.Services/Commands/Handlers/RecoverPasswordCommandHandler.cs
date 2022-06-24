using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMailService mailService;
        private readonly IPasswordHashService hashService;

        public RecoverPasswordCommandHandler(IUserRepository userRepository, IMailService mailService
            , IPasswordHashService hashService)
        {
            this.userRepository = userRepository;
            this.mailService = mailService;
            this.hashService = hashService;
        }
        public async Task<IResponse> Handle(RecoverPasswordCommand request, CancellationToken cancellationToken)
        {
            var newPass = Guid.NewGuid().ToString();
            var saltPass = hashService.GenerateSaltPass(newPass);
            var setPass = userRepository.UpdateUserPasswordByEmail(saltPass, request.Dto.Email);
            if (!setPass.IsSuccess) return setPass;
            await userRepository.SaveChangesAsync();
            await mailService.SendRecoveredPasswordAsync(request.Dto.Email, newPass);
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
