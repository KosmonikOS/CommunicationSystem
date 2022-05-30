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
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IContentResponse<List<UserAccountAdminDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetUsersQueryHandler> logger;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper
            , ILogger<GetUsersQueryHandler> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<UserAccountAdminDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<UserAccountAdminDto>(userRepository
                    .GetUsersPage(request.Page, request.Search, request.SearchOption))
                    .ToListAsync();
                return new ContentResponse<List<UserAccountAdminDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<UserAccountAdminDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
