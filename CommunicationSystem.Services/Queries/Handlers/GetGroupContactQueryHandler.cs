using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetGroupContactQueryHandler : IRequestHandler<GetGroupContactQuery, IContentResponse<ContactDto>>
    {
        private readonly IContactRepository contactRepository;

        public GetGroupContactQueryHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<IContentResponse<ContactDto>> Handle(GetGroupContactQuery request, CancellationToken cancellationToken)
        {
            var dto = contactRepository.GetGroupContact(request.FromGroup);
            if (dto == null)
                return new ContentResponse<ContactDto>(ResponseStatus.NotFound) { Message = "Чат не найден" };
            return new ContentResponse<ContactDto>(ResponseStatus.Ok) { Content = dto };
        }
    }
}
