using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class RegistrateUserCommand :IRequest<IResponse>
    {
        public RegistrationDto Dto { get; set; }
    }
}
