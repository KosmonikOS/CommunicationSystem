using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class ViewMessageCommandHandler : IRequestHandler<ViewMessageCommand, BaseResponse>
    {
        private readonly IMessageRepository messageRepository;

        public ViewMessageCommandHandler(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<BaseResponse> Handle(ViewMessageCommand request, CancellationToken cancellationToken)
        {
            messageRepository.ViewMessage(request.Id);
            await messageRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
