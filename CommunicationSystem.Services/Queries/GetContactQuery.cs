using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetContactQuery :IRequest<IContentResponse<ContactDto>>
    {
        public int From { get; set; }
    }
}
