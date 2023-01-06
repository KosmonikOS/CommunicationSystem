using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteUserByAdminCommandHandler : IRequestHandler<DeleteUserByAdminCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        public DeleteUserByAdminCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IResponse> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.DeleteUserAsync(request.Id);
            if (!result.IsSuccess) return result;
            await userRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}

