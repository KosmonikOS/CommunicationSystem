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
    public class GetCreateTestsQueryHandler : IRequestHandler<GetCreateTestsQuery, IContentResponse<List<CreateTestShowDto>>>
    {
        private readonly ICreateTestRepository testRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetCreateTestsQueryHandler> logger;

        public GetCreateTestsQueryHandler(ICreateTestRepository testRepository,IMapper mapper
            ,ILogger<GetCreateTestsQueryHandler> logger)
        {
            this.testRepository = testRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<CreateTestShowDto>>> Handle(GetCreateTestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<CreateTestShowDto>(testRepository
                    .GetUserTestsPage(request.UserId, request.Role
                    , request.Page, request.Search, request.SearchOption)).ToListAsync();
                return new ContentResponse<List<CreateTestShowDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<CreateTestShowDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
