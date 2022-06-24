using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateUserByAdminCommandHandler : IRequestHandler<UpdateUserByAdminCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPasswordHashService hashService;

        public UpdateUserByAdminCommandHandler(IUserRepository userRepository, IMapper mapper
            , IPasswordHashService hashService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.hashService = hashService;
        }
        public async Task<IResponse> Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
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
    }
}
