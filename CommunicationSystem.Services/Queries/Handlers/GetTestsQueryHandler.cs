using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetTestsQueryHandler : IRequestHandler<GetTestsQuery, IContentResponse<List<TestShowDto>>>
    {
        private readonly ITestRepository testRepository;
        private readonly IMapper mapper;

        public GetTestsQueryHandler(ITestRepository testRepository
            , IMapper mapper)
        {
            this.testRepository = testRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<TestShowDto>>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<TestShowDto>(testRepository
                .GetUserTestsPage(request.UserId, request.Page, request.Searh
                , request.SearchOption)).ToListAsync(cancellationToken);
            return new ContentResponse<List<TestShowDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
