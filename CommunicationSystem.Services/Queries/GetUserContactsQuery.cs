using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetUserContactsQuery : IRequest<IContentResponse<List<ContactDto>>>
    {
        public int UserId { get; set; }
    }
}
