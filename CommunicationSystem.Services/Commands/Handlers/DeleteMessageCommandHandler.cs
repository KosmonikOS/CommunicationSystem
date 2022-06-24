using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, BaseResponse>
    {
        private readonly IMessageRepository messageRepository;

        public DeleteMessageCommandHandler(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<BaseResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            messageRepository.DeleteMessage(request.Id);
            await messageRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
