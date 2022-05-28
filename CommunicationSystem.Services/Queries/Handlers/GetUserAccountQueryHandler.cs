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
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetUserAccountQueryHandler> logger;

        public GetUserAccountQueryHandler(IAccountRepository accountRepository,IMapper mapper
            ,ILogger<GetUserAccountQueryHandler> logger)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<UserAccountDto>> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = mapper.ProjectTo<UserAccountDto>(accountRepository.GetUsersByEmail(request.Email)
                    .Include(x => x.Role)).FirstOrDefault();
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
