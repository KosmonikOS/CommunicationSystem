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
    public class UpdateUserByAdminCommandHandler : IRequestHandler<UpdateUserByAdminCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPasswordHashService hashService;
        private readonly ILogger<AddUserByAdminCommandHandler> logger;

        public UpdateUserByAdminCommandHandler(IUserRepository userRepository, IMapper mapper
            , IPasswordHashService hashService, ILogger<AddUserByAdminCommandHandler> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.hashService = hashService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = mapper.Map<User>(request.Dto);
                if (!string.IsNullOrEmpty(request.Dto.Password))
                {
                    var hash = hashService.GenerateSaltPass(request.Dto.Password);
                    var passUpdateResult = userRepository.UpdateUserPasswordByEmail(hash, user.Email);
                    if (!passUpdateResult.IsSuccess) return passUpdateResult;
                }
                userRepository.UpdateUser(user, false);
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
