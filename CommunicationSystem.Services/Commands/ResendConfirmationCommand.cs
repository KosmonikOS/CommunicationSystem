using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class ResendConfirmationCommand :IRequest<IResponse>
    {
        public string Email { get; set; }
    }
}
