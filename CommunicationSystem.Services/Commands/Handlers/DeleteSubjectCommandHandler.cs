using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, IResponse>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly ILogger<AddSubjectCommandHandler> logger;

        public DeleteSubjectCommandHandler(ISubjectRepository subjectRepository, ILogger<AddSubjectCommandHandler> logger)
        {
            this.subjectRepository = subjectRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await subjectRepository.DeleteSubjectAsync(request.Id);
                if (!result.IsSuccess) return result;
                await subjectRepository.SaveChangesAsync();
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
