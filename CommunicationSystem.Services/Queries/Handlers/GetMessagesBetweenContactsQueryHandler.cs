using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetMessagesBetweenContactsQueryHandler : IRequestHandler<GetMessagesBetweenContactsQuery, IContentResponse<List<ContactMessageDto>>>
    {
        private readonly IMessageRepository messageRepository;

        public GetMessagesBetweenContactsQueryHandler(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<IContentResponse<List<ContactMessageDto>>> Handle(GetMessagesBetweenContactsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await messageRepository.GetMessagesBetweenContacts(request.UserId
                , request.ContactId,request.Page).ToListAsync();
            return new ContentResponse<List<ContactMessageDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
