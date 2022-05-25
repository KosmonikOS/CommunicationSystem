
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class GenerateEnterTokenCommandHandler : IRequestHandler<GenerateEnterTokenCommand, IContentResponse<AccessTokenDto>>
    {
        private readonly IAuthRepository authRepository;

        public GenerateEnterTokenCommandHandler(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
        public async Task<IContentResponse<AccessTokenDto>> Handle(GenerateEnterTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = authRepository.GetConfirmedUser(request.Dto);
                if (!user.IsSuccess) 
                    return new ContentResponse<AccessTokenDto>(user.Status) { Message = user.Message};

            }
            catch(Exception e)
            {
                return new ContentResponse<AccessTokenDto>(ResponseStatus.InternalServerError) { Message = e.Message };
            }
        }
    }
}