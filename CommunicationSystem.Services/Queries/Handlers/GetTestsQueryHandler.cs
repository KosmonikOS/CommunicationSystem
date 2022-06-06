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
    public class GetTestsQueryHandler : IRequestHandler<GetTestsQuery, IContentResponse<List<TestShowDto>>>
    {
        private readonly ITestRepository testRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetCreateTestsQueryHandler> logger;

        public GetTestsQueryHandler(ITestRepository testRepository
            ,IMapper mapper,ILogger<GetCreateTestsQueryHandler> logger)
        {
            this.testRepository = testRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<TestShowDto>>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<TestShowDto>(testRepository
                    .GetUserTestsPage(request.UserId,request.Page,request.Searh
                    ,request.SearchOption)).ToListAsync();
                return new ContentResponse<List<TestShowDto>>(ResponseStatus.Ok) { Content = dtos};
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<TestShowDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
