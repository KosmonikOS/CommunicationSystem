using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class RegistrateUserCommandHandler : IRequestHandler<RegistrateUserCommand, IResponse>
    {
        private readonly IConfirmationTokenService tokenService;
        private readonly IUserRepository userRepository;
        private readonly IMailService mailService;
        private readonly IPasswordHashService hashService;
        private readonly ILogger<RegistrateUserCommandHandler> logger;

        public RegistrateUserCommandHandler(IConfirmationTokenService tokenService
            , IUserRepository userRepository, IMailService mailService
            ,IPasswordHashService hashService, ILogger<RegistrateUserCommandHandler> logger)
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.mailService = mailService;
            this.hashService = hashService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(RegistrateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Dto != null)
                {
                    var user = userRepository.GetUsers(x => x.Email == request.Dto.Email)
                        .FirstOrDefault();
                    if (user != null)
                    {
                        logger.LogWarning("User with same email has already added");
                        return new BaseResponse(ResponseStatus.BadRequest) { Message = "Почта уже используется" };
                    }
                    var token = tokenService.GenerateToken(request.Dto.Email);
                    var saltPass = hashService.GenerateSaltPass(request.Dto.Password);
                    userRepository.AddUser(request.Dto,saltPass ,token);
                    await userRepository.SaveChangesAsync();
                    await mailService.SendConfirmationAsync(request.Dto.Email, token);
                    logger.LogInformation($"User with {request.Dto.Email} successfully created");
                    return new BaseResponse(ResponseStatus.Ok);
                }
                return new BaseResponse(ResponseStatus.BadRequest);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
