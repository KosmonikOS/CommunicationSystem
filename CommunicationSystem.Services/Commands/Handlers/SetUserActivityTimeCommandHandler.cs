using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class SetUserActivityTimeCommandHandler : IRequestHandler<SetUserActivityTimeCommand, IResponse>
    {
        private readonly IAuthRepository authRepository;

        public SetUserActivityTimeCommandHandler(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
        public async Task<IResponse> Handle(SetUserActivityTimeCommand request, CancellationToken cancellationToken)
        {
            authRepository.SetTime(request.Dto);
            await authRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
