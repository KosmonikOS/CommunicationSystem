using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountQuery, IContentResponse<UserAccountDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserAccountQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<UserAccountDto>> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
        {
            var dto = mapper.ProjectTo<UserAccountDto>(userRepository
                .GetUsers(x => x.Email == request.Email)).FirstOrDefault();
            return new ContentResponse<UserAccountDto>(ResponseStatus.Ok) { Content = dto };
        }
    }
}
