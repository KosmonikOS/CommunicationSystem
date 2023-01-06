using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateMessageContentCommand : IRequest<BaseResponse>
    {
        public MessageContentUpdateDto Dto { get; set; }
    }
}
