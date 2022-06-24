using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetGroupByIdQuery :IRequest<IContentResponse<GroupShowDto>>
    {
        public Guid Id { get; set; }
    }
}
