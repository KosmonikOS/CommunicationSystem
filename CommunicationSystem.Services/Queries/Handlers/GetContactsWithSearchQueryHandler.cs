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
    public class GetContactsWithSearchQueryHandler : IRequestHandler<GetContactsWithSearchQuery, IContentResponse<List<ContactSearchDto>>>
    {
        private readonly IUserRepository userRepository;

        public GetContactsWithSearchQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IContentResponse<List<ContactSearchDto>>> Handle(GetContactsWithSearchQuery request, CancellationToken cancellationToken)
        {
            var dtos = await userRepository.GetUsersWithSearch(request.Search, UserSearchOption.NickName)
                .Select(x => new ContactSearchDto()
                {
                    ToId = x.Id,
                    AccountImage = x.AccountImage,
                    NickName = x.NickName,
                    LastActivity = Data.UserFunctions.GetUserLastActivity(x.Id)
                }).ToListAsync(cancellationToken);
            return new ContentResponse<List<ContactSearchDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
