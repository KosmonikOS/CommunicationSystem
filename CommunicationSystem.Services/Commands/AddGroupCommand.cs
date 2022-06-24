using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class AddGroupCommand :IRequest<IResponse>
    {
        public CreateGroupDto Dto { get; set; }
    }
}
