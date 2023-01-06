using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetGroupMemberWithSearchQuery :IRequest<IContentResponse<List<GroupSearchMemberDto>>>
    {
        public string Search { get; set; }
    }
}
