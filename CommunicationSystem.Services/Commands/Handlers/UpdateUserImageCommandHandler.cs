using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateUserImageCommandHandler : IRequestHandler<UpdateUserImageCommand, IContentResponse<string>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IFileService fileService;
        private readonly ILogger<UpdateUserImageCommandHandler> logger;

        public UpdateUserImageCommandHandler(IAccountRepository accountRepository,IFileService fileService
            ,ILogger<UpdateUserImageCommandHandler> logger)
        {
            this.accountRepository = accountRepository;
            this.fileService = fileService;
            this.logger = logger;
        }
        public async Task<IContentResponse<string>> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ImageToSave == null)
                    return new ContentResponse<string>(ResponseStatus.BadRequest) { Message = "Некорректный формат файла" };
                var path = await fileService.SaveFileAsync(request.ImageToSave);
                var updateImage = accountRepository.UpdateImage(request.Id, path);
                if (!updateImage.IsSuccess)
                    return new ContentResponse<string>(updateImage.Status) { Message = updateImage.Message };
                await accountRepository.SaveChangesAsync();
                return new ContentResponse<string>(ResponseStatus.Ok) { Content = path };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<string>(ResponseStatus.InternalServerError) { Message = e.Message };
            }
        }
    }
}
