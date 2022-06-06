using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddStudentAnswersWithCheckCommandHandler : IRequestHandler<AddStudentAnswersWithCheckCommand, IResponse>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IStudentMarkService markService;
        private readonly ILogger<AddStudentAnswersWithCheckCommandHandler> logger;

        public AddStudentAnswersWithCheckCommandHandler(IStudentRepository studentRepository
            , IStudentMarkService markService, ILogger<AddStudentAnswersWithCheckCommandHandler> logger)
        {
            this.studentRepository = studentRepository;
            this.markService = markService;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(AddStudentAnswersWithCheckCommand request, CancellationToken cancellationToken)
        {
            try
            {
                studentRepository.AddStudentAnswers(request.Dto);
                var mark = await markService.CalculateStudentMarkAsync(request.Dto);
                if (mark.IsSuccess)
                    studentRepository.UpdateStudentMark(new UpdateStudentMarkDto()
                    {
                        TestId = request.Dto.TestId,
                        UserId = request.Dto.UserId,
                        Mark = mark.Content,
                    });
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
