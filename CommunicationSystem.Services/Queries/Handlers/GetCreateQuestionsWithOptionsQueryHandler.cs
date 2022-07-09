using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetCreateQuestionsWithOptionsQueryHandler : IRequestHandler<GetCreateQuestionsWithOptionsQuery, IContentResponse<List<CreateQuestionDto>>>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;

        public GetCreateQuestionsWithOptionsQueryHandler(IQuestionRepository questionRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<CreateQuestionDto>>> Handle(GetCreateQuestionsWithOptionsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<CreateQuestionDto>(questionRepository
                .GetQuestions(request.TestId)).ToListAsync(cancellationToken);
            return new ContentResponse<List<CreateQuestionDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
