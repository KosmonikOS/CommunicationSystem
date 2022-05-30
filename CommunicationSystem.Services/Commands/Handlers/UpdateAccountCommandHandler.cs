using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, IResponse>
    {
        private readonly IPasswordHashService hashService;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly ILogger<UpdateAccountCommandHandler> logger;

        public UpdateAccountCommandHandler(IPasswordHashService hashService, IMapper mapper
            , IUserRepository userRepository, ILogger<UpdateAccountCommandHandler> logger)
        {
            this.hashService = hashService;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = mapper.Map<User>(request.Dto);
                if (!string.IsNullOrEmpty(request.Dto.Password))
                {
                    var hash = hashService.GenerateSaltPass(request.Dto.Password);
                    userRepository.UpdateUserPasswordByEmail(hash, user.Email);
                }
                userRepository.UpdateUser(user,false);
                await userRepository.SaveChangesAsync();
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
