using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddUserByAdminCommandHandler : IRequestHandler<AddUserByAdminCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPasswordHashService hashService;

        public AddUserByAdminCommandHandler(IUserRepository userRepository, IMapper mapper
            , IPasswordHashService hashService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.hashService = hashService;
        }
        public async Task<IResponse> Handle(AddUserByAdminCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request.Dto);
            var saltPass = hashService.GenerateSaltPass(request.Dto.Password);
            user.PassHash = saltPass;
            user.IsConfirmed = "true";
            userRepository.AddUser(user);
            await userRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
