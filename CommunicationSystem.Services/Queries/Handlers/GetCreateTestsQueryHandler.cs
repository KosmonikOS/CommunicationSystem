using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetCreateTestsQueryHandler : IRequestHandler<GetCreateTestsQuery, IContentResponse<List<CreateTestShowDto>>>
    {
        private readonly ITestRepository testRepository;
        private readonly IMapper mapper;

        public GetCreateTestsQueryHandler(ITestRepository testRepository, IMapper mapper)
        {
            this.testRepository = testRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<CreateTestShowDto>>> Handle(GetCreateTestsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<CreateTestShowDto>(testRepository
                .GetUserCreateTestsPage(request.UserId, request.Role
                , request.Page, request.Search, request.SearchOption)).ToListAsync(cancellationToken);
            return new ContentResponse<List<CreateTestShowDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
