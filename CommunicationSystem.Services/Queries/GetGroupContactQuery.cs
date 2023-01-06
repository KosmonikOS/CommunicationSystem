using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetGroupContactQuery :IRequest<IContentResponse<ContactDto>>
    {
        public Guid FromGroup { get; set; }
    }
}
