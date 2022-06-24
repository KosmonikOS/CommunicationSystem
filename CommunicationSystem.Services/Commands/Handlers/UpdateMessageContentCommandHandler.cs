using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateMessageContentCommandHandler : IRequestHandler<UpdateMessageContentCommand, BaseResponse>
    {
        private readonly IMessageRepository messageRepository;

        public UpdateMessageContentCommandHandler(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<BaseResponse> Handle(UpdateMessageContentCommand request, CancellationToken cancellationToken)
        {
            messageRepository.UpdateMessageContent(request.Dto.Id, request.Dto.Content);
            await messageRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
