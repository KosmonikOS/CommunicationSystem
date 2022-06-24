using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetGroupMemberWithSearchQueryHandler : IRequestHandler<GetGroupMemberWithSearchQuery, IContentResponse<List<GroupSearchMemberDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetGroupMemberWithSearchQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<GroupSearchMemberDto>>> Handle(GetGroupMemberWithSearchQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<GroupSearchMemberDto>(userRepository
                .GetUsersWithSearch(request.Search, UserSearchOption.NickName)).ToListAsync();
            return new ContentResponse<List<GroupSearchMemberDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
