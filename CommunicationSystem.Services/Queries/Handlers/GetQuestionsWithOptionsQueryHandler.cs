using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetQuestionsWithOptionsQueryHandler : IRequestHandler<GetQuestionsWithOptionsQuery, IContentResponse<List<CreateQuestionDto>>>
    {
        private readonly ICreateQuestionRepository questionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetQuestionsWithOptionsQueryHandler> logger;

        public GetQuestionsWithOptionsQueryHandler(ICreateQuestionRepository questionRepository
            ,IMapper mapper, ILogger<GetQuestionsWithOptionsQueryHandler> logger)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<CreateQuestionDto>>> Handle(GetQuestionsWithOptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<CreateQuestionDto>(questionRepository
                    .GetQuestions(request.TestId)).ToListAsync();
                return new ContentResponse<List<CreateQuestionDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<CreateQuestionDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
