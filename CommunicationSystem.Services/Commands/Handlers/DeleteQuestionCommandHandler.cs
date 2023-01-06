using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, IResponse>
    {
        private readonly IQuestionRepository questionRepository;

        public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }
        public async Task<IResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var result = await questionRepository.DeleteQuestionAsync(request.QuestionId);
            if (!result.IsSuccess) return result;
            await questionRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
