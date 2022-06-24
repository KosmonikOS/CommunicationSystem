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
    public class AddFileMessageCommandHandler : IRequestHandler<AddFileMessageCommand, IContentResponse<FileMessageResponseDto>>
    {
        private readonly IFileService fileService;
        private readonly IMessageService messageService;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public AddFileMessageCommandHandler(IFileService fileService, IMessageService messageService
            , IMessageRepository messageRepository, IMapper mapper)
        {
            this.fileService = fileService;
            this.messageService = messageService;
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<FileMessageResponseDto>> Handle(AddFileMessageCommand request, CancellationToken cancellationToken)
        {
            var form = await fileService.SaveStreamFileWithFormDataAsync(request
                .ControllerContext.Request);
            AddFileMessageDto dto = new AddFileMessageDto();
            if (await request.ControllerContext.TryUpdateModelAsync(dto, "", form)
                || request.ControllerContext.TryValidateModel(dto))
                return new ContentResponse<FileMessageResponseDto>(ResponseStatus.BadRequest) { Message = "Некорректный формат данных" };
            var message = mapper.Map<Message>(dto);
            message.Type = fileService.IsImage(dto.FileType)
                ? MessageType.Image : MessageType.File;
            message.Date = DateTime.UtcNow;
            messageRepository.AddMessage(message);
            await messageRepository.SaveChangesAsync();
            var responseDto = mapper.Map<SendMessageDto>(message);
            await messageService.SendMessageAsync(responseDto);
            return new ContentResponse<FileMessageResponseDto>(ResponseStatus.Ok)
            {
                Content = new FileMessageResponseDto()
                {
                    Id = message.Id,
                    Path = message.Content,
                    Type = message.Type
                }
            };
        }
    }
}
