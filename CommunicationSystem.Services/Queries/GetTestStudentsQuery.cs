using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetTestStudentsQuery :IRequest<IContentResponse<List<TestStudentDto>>>
    {
        public Guid TestId { get; set; }
    }
}
