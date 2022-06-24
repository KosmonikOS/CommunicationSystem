using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand, IResponse>
    {
        private readonly ITestRepository testRepository;

        public DeleteTestCommandHandler(ITestRepository testRepository)
        {
            this.testRepository = testRepository;
        }
        public async Task<IResponse> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            var result = await testRepository.DeleteTestAsync(request.TestId);
            if (!result.IsSuccess) return result;
            await testRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
