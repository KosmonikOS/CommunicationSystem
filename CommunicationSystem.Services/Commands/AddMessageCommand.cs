using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class AddMessageCommand : IRequest<IContentResponse<int>>
    {
        public AddMessageDto Dto { get; set; }
    }
}
