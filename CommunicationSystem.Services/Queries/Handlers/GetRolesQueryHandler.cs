using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IContentResponse<List<Role>>>
    {
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<GetRolesQueryHandler> logger;

        public GetRolesQueryHandler(IRoleRepository roleRepository,ILogger<GetRolesQueryHandler> logger)
        {
            this.roleRepository = roleRepository;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = roleRepository.GetRoles().ToList();
                return new ContentResponse<List<Role>>(ResponseStatus.Ok) { Content = roles };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<Role>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
