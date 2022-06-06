using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, IResponse>
    {
        private readonly IOptionRepository optionRepository;
        private readonly ILogger<DeleteOptionCommandHandler> logger;

        public DeleteOptionCommandHandler(IOptionRepository optionRepository, ILogger<DeleteOptionCommandHandler> logger)
        {
            this.optionRepository = optionRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = optionRepository.DeleteOption(request.OptionId);
                if (!result.IsSuccess) return result;
                await optionRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
