using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class GenerateEnterTokenCommand : IRequest<IContentResponse<TokenPairDto>>
    {
        public LoginDto Dto { get; set; }
    }
}
