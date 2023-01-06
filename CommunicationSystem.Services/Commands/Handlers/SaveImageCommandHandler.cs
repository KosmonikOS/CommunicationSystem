using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class SaveImageCommandHandler : IRequestHandler<SaveImageCommand, IContentResponse<string>>
    {
        private readonly IFileService fileService;

        public SaveImageCommandHandler(IFileService fileService)
        {
            this.fileService = fileService;
        }
        public async Task<IContentResponse<string>> Handle(SaveImageCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null)
                return new ContentResponse<string>(ResponseStatus.BadRequest) { Message = "Некорректный формат файла" };
            var path = await fileService.SaveFileAsync(request.File);
            return new ContentResponse<string>(ResponseStatus.Ok) { Content = path };
        }
    }
}
