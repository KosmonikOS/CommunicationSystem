using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteTestCommand :IRequest<IResponse>
    {
        public Guid TestId { get; set; }
    }
}
