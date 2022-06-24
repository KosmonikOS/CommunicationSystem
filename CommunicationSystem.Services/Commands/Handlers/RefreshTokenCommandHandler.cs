using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, IContentResponse<RefreshTokenDto>>
    {
        private readonly IJwtService jwtService;

        public RefreshTokenCommandHandler(IJwtService jwtService)
        {
            this.jwtService = jwtService;
        }
        public async Task<IContentResponse<RefreshTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshedToken = await jwtService.RefreshAsync(request.Dto);
            return refreshedToken;
        }
    }
}
