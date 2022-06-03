using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateStudentMarkCommandHandler : IRequestHandler<UpdateStudentMarkCommand, IResponse>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ILogger<UpdateStudentMarkCommandHandler> logger;

        public UpdateStudentMarkCommandHandler(IStudentRepository studentRepository,ILogger<UpdateStudentMarkCommandHandler> logger)
        {
            this.studentRepository = studentRepository;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(UpdateStudentMarkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = studentRepository.UpdateStudentMark(request.Dto);
                if (!result.IsSuccess) return result;
                await studentRepository.SaveChangesAsync();
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
