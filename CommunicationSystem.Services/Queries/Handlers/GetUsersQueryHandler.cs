using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IContentResponse<List<UserAccountAdminDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<UserAccountAdminDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<UserAccountAdminDto>(userRepository
                .GetUsersPage(request.Page, request.Search, request.SearchOption))
                .ToListAsync(cancellationToken);
            return new ContentResponse<List<UserAccountAdminDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
