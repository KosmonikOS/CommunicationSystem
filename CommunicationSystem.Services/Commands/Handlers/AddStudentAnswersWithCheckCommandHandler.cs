using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddStudentAnswersWithCheckCommandHandler : IRequestHandler<AddStudentAnswersWithCheckCommand, IResponse>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IStudentMarkService markService;

        public AddStudentAnswersWithCheckCommandHandler(IStudentRepository studentRepository
            , IStudentMarkService markService)
        {
            this.studentRepository = studentRepository;
            this.markService = markService;
        }
        public async Task<IResponse> Handle(AddStudentAnswersWithCheckCommand request, CancellationToken cancellationToken)
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
    }
}
