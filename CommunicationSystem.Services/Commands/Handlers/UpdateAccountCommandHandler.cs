using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, IResponse>
    {
        private readonly IPasswordHashService hashService;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateAccountCommandHandler(IPasswordHashService hashService, IMapper mapper
            , IUserRepository userRepository)
        {
            this.hashService = hashService;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }
        public async Task<IResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request.Dto);
            if (!string.IsNullOrEmpty(request.Dto.Password))
            {
                var hash = hashService.GenerateSaltPass(request.Dto.Password);
                userRepository.UpdateUserPasswordByEmail(hash, user.Email);
            }
            userRepository.UpdateUser(user, false);
            await userRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
