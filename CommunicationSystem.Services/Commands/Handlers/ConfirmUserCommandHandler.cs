
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class ConfirmUserCommandHandler : IRequestHandler<ConfirmUserCommand, IResponse>
    {
        private readonly IConfirmationTokenService tokenService;
        private readonly ILogger<ConfirmUserCommandHandler> logger;

        public ConfirmUserCommandHandler(IConfirmationTokenService tokenService
            ,ILogger<ConfirmUserCommandHandler> logger)
        {
            this.tokenService = tokenService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await tokenService.ConfirmTokenAsync(request.Token);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError) { Message = e.Message };
            }
        }
    }
}
