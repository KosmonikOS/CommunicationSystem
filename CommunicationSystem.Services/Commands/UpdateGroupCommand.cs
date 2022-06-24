using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateGroupCommand : IRequest<IResponse>
    {
        public CreateGroupDto Dto { get; set; }
    }
}
