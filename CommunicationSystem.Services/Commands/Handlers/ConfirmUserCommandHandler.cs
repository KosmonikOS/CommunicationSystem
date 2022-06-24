using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class ConfirmUserCommandHandler : IRequestHandler<ConfirmUserCommand, IResponse>
    {
        private readonly IConfirmationTokenService tokenService;

        public ConfirmUserCommandHandler(IConfirmationTokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        public async Task<IResponse> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
        {
            return await tokenService.ConfirmTokenAsync(request.Token);
        }
    }
}
