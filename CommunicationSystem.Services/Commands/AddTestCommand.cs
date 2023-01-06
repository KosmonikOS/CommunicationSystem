using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class AddTestCommand : IRequest<IResponse>
    {
        public CreateTestDto Dto { get; set; }
    }
}
