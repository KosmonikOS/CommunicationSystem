using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetStudentAnswersQuery :IRequest<IContentResponse<List<StudentAnswerShowDto>>>
    {
        public int UserId { get; set; }
        public Guid TestId { get; set; }
    }
}
