using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetMessagesBetweenContactsQuery : IRequest<IContentResponse<List<ContactMessageDto>>>
    {
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public int Page { get; set; }
    }
}
