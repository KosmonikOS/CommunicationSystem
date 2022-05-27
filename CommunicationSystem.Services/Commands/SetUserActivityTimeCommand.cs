using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class SetUserActivityTimeCommand :IRequest<IResponse>
    {
        public UserActivityDto Dto { get; set; }
    }
}
