using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetStudentsWithSearchQueryHandler : IRequestHandler<GetStudentsWithSearchQuery, IContentResponse<List<SearchStudentDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetStudentsWithSearchQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<SearchStudentDto>>> Handle(GetStudentsWithSearchQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<SearchStudentDto>(userRepository
                .GetUsersWithSearch(request.Search, request.SearchOption)).ToListAsync(cancellationToken);
            return new ContentResponse<List<SearchStudentDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
