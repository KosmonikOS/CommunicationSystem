using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetQuestionsWithOptionsQueryHandler : IRequestHandler<GetQuestionsWithOptionsQuery, IContentResponse<List<QuestionShowDto>>>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public GetQuestionsWithOptionsQueryHandler(IQuestionRepository questionRepository
            , IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<QuestionShowDto>>> Handle(GetQuestionsWithOptionsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<QuestionShowDto>(questionRepository
                .GetQuestions(request.TestId)).ToListAsync();
            return new ContentResponse<List<QuestionShowDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
