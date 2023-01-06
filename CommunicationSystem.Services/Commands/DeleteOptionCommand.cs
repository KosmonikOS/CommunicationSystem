using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteOptionCommand :IRequest<IResponse>
    {
        public Guid OptionId { get; set; }
    }
}
