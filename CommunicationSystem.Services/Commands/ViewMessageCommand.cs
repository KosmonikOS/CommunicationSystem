using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class ViewMessageCommand : IRequest<BaseResponse>
    {
        public int Id { get; set; }
    }
}
