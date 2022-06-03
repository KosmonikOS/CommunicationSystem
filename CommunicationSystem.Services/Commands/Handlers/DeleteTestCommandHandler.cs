using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand, IResponse>
    {
        private readonly ICreateTestRepository testRepository;
        private readonly ILogger<DeleteTestCommandHandler> logger;

        public DeleteTestCommandHandler(ICreateTestRepository testRepository, ILogger<DeleteTestCommandHandler> logger)
        {
            this.testRepository = testRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await testRepository.DeleteTestAsync(request.TestId);
                if (!result.IsSuccess) return result;
                await testRepository.SaveChangesAsync();
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
