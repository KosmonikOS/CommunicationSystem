using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteUserByAdminCommandHandler : IRequestHandler<DeleteUserByAdminCommand, IResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<DeleteUserByAdminCommandHandler> logger;
        public DeleteUserByAdminCommandHandler(IUserRepository userRepository,ILogger<DeleteUserByAdminCommandHandler> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await userRepository.DeleteUserAsync(request.Id);
                if (!result.IsSuccess) return result;
                await userRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
