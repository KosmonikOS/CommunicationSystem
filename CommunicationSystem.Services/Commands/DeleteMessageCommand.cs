using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteMessageCommand : IRequest<BaseResponse>
    {
        public int Id { get; set; }
    }
}
