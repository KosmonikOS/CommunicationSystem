
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class SaveImageCommandHandler : IRequestHandler<SaveImageCommand, IContentResponse<string>>
    {
        private readonly IFileService fileService;
        private readonly ILogger<SaveImageCommandHandler> logger;

        public SaveImageCommandHandler(IFileService fileService
            ,ILogger<SaveImageCommandHandler> logger)
        {
            this.fileService = fileService;
            this.logger = logger;
        }
        public async Task<IContentResponse<string>> Handle(SaveImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null)
                    return new ContentResponse<string>(ResponseStatus.BadRequest) { Message = "Некорректный формат файла" };
                var path = await fileService.SaveFileAsync(request.File);
                return new ContentResponse<string>(ResponseStatus.Ok) { Content = path };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<string>(ResponseStatus.InternalServerError);
            }
        }
    }
}
