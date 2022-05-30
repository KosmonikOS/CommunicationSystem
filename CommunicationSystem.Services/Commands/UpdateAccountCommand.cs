using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateAccountCommand :IRequest<IResponse>
    {
        public UserAccountUpdateDto Dto { get; set; }
    }
}
