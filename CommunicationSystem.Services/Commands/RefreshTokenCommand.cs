using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class RefreshTokenCommand :IRequest<IContentResponse<RefreshTokenDto>>
    {
        public RefreshTokenDto Dto { get; set; }
    }
}
