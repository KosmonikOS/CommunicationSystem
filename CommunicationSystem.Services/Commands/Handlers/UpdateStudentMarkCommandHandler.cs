using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateStudentMarkCommandHandler : IRequestHandler<UpdateStudentMarkCommand, IResponse>
    {
        private readonly IStudentRepository studentRepository;

        public UpdateStudentMarkCommandHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        public async Task<IResponse> Handle(UpdateStudentMarkCommand request, CancellationToken cancellationToken)
        {
            var result = studentRepository.UpdateStudentMark(request.Dto);
            if (!result.IsSuccess) return result;
            await studentRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
