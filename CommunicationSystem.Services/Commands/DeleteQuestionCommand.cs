using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteQuestionCommand :IRequest<IResponse>
    {
        public Guid QuestionId { get; set; }
    }
}
