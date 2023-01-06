using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IContentResponse<List<Role>>>
    {
        private readonly IRoleRepository roleRepository;

        public GetRolesQueryHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }
        public async Task<IContentResponse<List<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = roleRepository.GetRoles().ToList();
            return new ContentResponse<List<Role>>(ResponseStatus.Ok) { Content = roles };
        }
    }
}
