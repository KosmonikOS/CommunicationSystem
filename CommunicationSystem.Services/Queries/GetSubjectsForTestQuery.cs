using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetSubjectsForTestQuery :IRequest<IContentResponse<List<Subject>>>
    {
    }
}
