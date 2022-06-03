using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, IResponse>
    {
        private readonly ICreateQuestionRepository questionRepository;
        private readonly ILogger<DeleteQuestionCommandHandler> logger;

        public DeleteQuestionCommandHandler(ICreateQuestionRepository questionRepository, ILogger<DeleteQuestionCommandHandler> logger)
        {
            this.questionRepository = questionRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await questionRepository.DeleteQuestionAsync(request.QuestionId);
                if (!result.IsSuccess) return result;
                await questionRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
