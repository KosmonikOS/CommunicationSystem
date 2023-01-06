using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class GenerateEnterTokenCommandHandler : IRequestHandler<GenerateEnterTokenCommand, IContentResponse<TokenPairDto>>
    {
        private readonly IAuthRepository authRepository;
        private readonly IJwtService jwtService;

        public GenerateEnterTokenCommandHandler(IAuthRepository authRepository, IJwtService jwtService)
        {
            this.authRepository = authRepository;
            this.jwtService = jwtService;
        }
        public async Task<IContentResponse<TokenPairDto>> Handle(GenerateEnterTokenCommand request, CancellationToken cancellationToken)
        {
            var user = authRepository.GetConfirmedUser(request.Dto);
            if (!user.IsSuccess)
                return new ContentResponse<TokenPairDto>(user.Status) { Message = user.Message };
            var rt = await jwtService.GenerateRTAsync(user.Content.Id);
            if (!rt.IsSuccess)
                return new ContentResponse<TokenPairDto>(rt.Status) { Message = rt.Message };
            var claims = jwtService.GenerateClaims(user.Content);
            var jwt = jwtService.GenerateJWT(claims);
            return new ContentResponse<TokenPairDto>(ResponseStatus.Ok)
            {
                Content = new TokenPairDto()
                {
                    AccessToken = jwt,
                    RefreshToken = rt.Content,
                    CurrentAccountId = user.Content.Id
                }
            };
        }
    }
}