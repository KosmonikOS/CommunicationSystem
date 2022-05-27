using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, IContentResponse<RefreshTokenDto>>
    {
        private readonly IJwtService jwtService;
        private readonly ILogger<RefreshTokenCommandHandler> logger;

        public RefreshTokenCommandHandler(IJwtService jwtService, ILogger<RefreshTokenCommandHandler> logger)
        {
            this.jwtService = jwtService;
            this.logger = logger;
        }
        public async Task<IContentResponse<RefreshTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshedToken = await jwtService.RefreshAsync(request.Dto);
                return refreshedToken;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<RefreshTokenDto>(ResponseStatus.InternalServerError);
            }
        }
    }
}
