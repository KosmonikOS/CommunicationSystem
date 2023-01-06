using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, IResponse>
    {
        private readonly IOptionRepository optionRepository;

        public DeleteOptionCommandHandler(IOptionRepository optionRepository)
        {
            this.optionRepository = optionRepository;
        }
        public async Task<IResponse> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
        {
            var result = optionRepository.DeleteOption(request.OptionId);
            if (!result.IsSuccess) return result;
            await optionRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
