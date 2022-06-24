using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetContactsWithSearchQuery : IRequest<IContentResponse<List<ContactSearchDto>>>
    {
        public string Search { get; set; }
    }
}
