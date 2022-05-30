using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetUsersQuery :IRequest<IContentResponse<List<UserAccountAdminDto>>>
    {
        public int Page { get; set; }
        public UserSearchOption SearchOption { get; set; }
        public string Search { get; set; }
    }
}
