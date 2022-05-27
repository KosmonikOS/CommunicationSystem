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
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IResponse>
    {
        private readonly IPasswordHashService hashService;
        private readonly IMapper mapper;
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<UpdateUserCommandHandler> logger;

        public UpdateUserCommandHandler(IPasswordHashService hashService, IMapper mapper
            , IAccountRepository accountRepository, ILogger<UpdateUserCommandHandler> logger)
        {
            this.hashService = hashService;
            this.mapper = mapper;
            this.accountRepository = accountRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = mapper.Map<User>(request.Dto);
                if (!string.IsNullOrEmpty(request.Dto.Password))
                {
                    var hash = hashService.GenerateSaltPass(request.Dto.Password);
                    accountRepository.UpdateUserPasswordByEmail(hash, user.Email);
                }
                accountRepository.UpdateUser(user);
                await accountRepository.SaveChangesAsync();
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
