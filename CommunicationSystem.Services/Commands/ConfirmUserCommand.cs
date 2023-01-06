
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class ConfirmUserCommand : IRequest<IResponse>
    {
        public string Token { get; set; }
    }
}
