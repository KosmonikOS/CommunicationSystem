using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetQuestionsWithOptionsQuery :IRequest<IContentResponse<List<QuestionShowDto>>>
    {
        public Guid TestId { get; set; }
    }
}
