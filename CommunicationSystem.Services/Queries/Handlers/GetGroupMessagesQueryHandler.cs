using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetGroupMessagesQueryHandler : IRequestHandler<GetGroupMessagesQuery, IContentResponse<List<GroupMessageDto>>>
    {
        private readonly IMessageRepository messageRepository;

        public GetGroupMessagesQueryHandler(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<IContentResponse<List<GroupMessageDto>>> Handle(GetGroupMessagesQuery request, CancellationToken cancellationToken)
        {
            var dtos = await messageRepository.GetGroupMessages(request.UserId,
                request.GroupId, request.Page).ToListAsync(cancellationToken);
            return new ContentResponse<List<GroupMessageDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
