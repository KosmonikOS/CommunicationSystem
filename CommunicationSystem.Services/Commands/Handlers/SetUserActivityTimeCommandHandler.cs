using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class SetUserActivityTimeCommandHandler : IRequestHandler<SetUserActivityTimeCommand, IResponse>
    {
        private readonly IAuthRepository authRepository;
        private readonly ILogger<SetUserActivityTimeCommandHandler> logger;

        public SetUserActivityTimeCommandHandler(IAuthRepository authRepository
            ,ILogger<SetUserActivityTimeCommandHandler> logger)
        {
            this.authRepository = authRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(SetUserActivityTimeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                authRepository.SetTime(request.Dto);
                await authRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError) { Message = e.Message };
            }
        }
    }
}
