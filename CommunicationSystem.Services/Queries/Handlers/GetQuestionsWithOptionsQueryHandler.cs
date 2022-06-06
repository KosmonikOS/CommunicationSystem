﻿using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetQuestionsWithOptionsQueryHandler : IRequestHandler<GetQuestionsWithOptionsQuery, IContentResponse<List<QuestionShowDto>>>
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetCreateQuestionsWithOptionsQueryHandler> logger;

        public GetQuestionsWithOptionsQueryHandler(IQuestionRepository questionRepository
            ,IMapper mapper, ILogger<GetCreateQuestionsWithOptionsQueryHandler> logger)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<QuestionShowDto>>> Handle(GetQuestionsWithOptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<QuestionShowDto>(questionRepository
                    .GetQuestions(request.TestId)).ToListAsync();
                return new ContentResponse<List<QuestionShowDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<QuestionShowDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
