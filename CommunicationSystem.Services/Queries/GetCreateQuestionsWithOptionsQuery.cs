using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetCreateQuestionsWithOptionsQuery :IRequest<IContentResponse<List<CreateQuestionDto>>>
    {
        public Guid TestId { get; set; }
    }
}
