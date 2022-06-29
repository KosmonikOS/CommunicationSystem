using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, IContentResponse<int>>
    {
        private readonly IMessageRepository messageRepository;
        private readonly IMessageService messageService;
        private readonly IMapper mapper;

        public AddMessageCommandHandler(IMessageRepository messageRepository
            , IMessageService messageService, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.messageService = messageService;
            this.mapper = mapper;
        }

        public async Task<IContentResponse<int>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var message = mapper.Map<Message>(request.Dto);
            message.Date = DateTime.UtcNow;
            message.Type = MessageType.Text;
            messageRepository.AddMessage(message);
            await messageRepository.SaveChangesAsync();
            var dto = mapper.Map<SendMessageDto>(message);
            if(message.ToId != message.FromId)
                await messageService.SendMessageAsync(dto);
            return new ContentResponse<int>(ResponseStatus.Ok) { Content = message.Id };
        }
    }
}
