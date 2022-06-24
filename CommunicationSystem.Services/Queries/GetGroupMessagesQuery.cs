using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetGroupMessagesQuery : IRequest<IContentResponse<List<GroupMessageDto>>>
    {
        public int Page { get; set; }
        public Guid GroupId { get; set; }
        public int UserId { get; set; }
    }
}
