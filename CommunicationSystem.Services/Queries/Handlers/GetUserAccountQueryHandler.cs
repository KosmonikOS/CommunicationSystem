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
    public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountQuery, IContentResponse<UserAccountDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetUserAccountQueryHandler> logger;

        public GetUserAccountQueryHandler(IUserRepository userRepository,IMapper mapper
            ,ILogger<GetUserAccountQueryHandler> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<UserAccountDto>> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = mapper.ProjectTo<UserAccountDto>(userRepository
                    .GetUsers(x => x.Email == request.Email)).FirstOrDefault();
                return new ContentResponse<UserAccountDto>(ResponseStatus.Ok) { Content = dto};
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<UserAccountDto>(ResponseStatus.InternalServerError);
            }
        }
    }
}
