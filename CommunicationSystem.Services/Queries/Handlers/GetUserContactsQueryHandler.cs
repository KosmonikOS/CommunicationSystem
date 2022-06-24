using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetUserContactsQueryHandler : IRequestHandler<GetUserContactsQuery, IContentResponse<List<ContactDto>>>
    {
        private readonly IContactRepository contactRepository;

        public GetUserContactsQueryHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<IContentResponse<List<ContactDto>>> Handle(GetUserContactsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await contactRepository.GetUserContacts(request.UserId).ToListAsync();
            return new ContentResponse<List<ContactDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
