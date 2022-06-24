using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, IResponse>
    {
        private readonly ISubjectRepository subjectRepository;

        public DeleteSubjectCommandHandler(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }
        public async Task<IResponse> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var result = await subjectRepository.DeleteSubjectAsync(request.Id);
            if (!result.IsSuccess) return result;
            await subjectRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
