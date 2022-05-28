using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetSubjectsQuery :IRequest<IContentResponse<List<Subject>>>
    {
        public string Search { get; set; }
        public int Page { get; set; }
    }
}
