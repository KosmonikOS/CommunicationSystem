using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetContactQueryHandler : IRequestHandler<GetContactQuery, IContentResponse<ContactDto>>
    {
        private readonly IContactRepository contactRepository;

        public GetContactQueryHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<IContentResponse<ContactDto>> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            var dto = contactRepository.GetContact(request.From);
            if (dto == null)
                return new ContentResponse<ContactDto>(ResponseStatus.NotFound) { Message = "Чат не найден" };
            return new ContentResponse<ContactDto>(ResponseStatus.Ok) { Content = dto };
        }
    }
}
