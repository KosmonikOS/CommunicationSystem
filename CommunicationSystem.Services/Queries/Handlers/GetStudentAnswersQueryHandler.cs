using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetStudentAnswersQueryHandler : IRequestHandler<GetStudentAnswersQuery, IContentResponse<List<StudentAnswerShowDto>>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ILogger<GetTestStudentsQueryHandler> logger;

        public GetStudentAnswersQueryHandler(IStudentRepository studentRepository
            ,ILogger<GetTestStudentsQueryHandler> logger)
        {
            this.studentRepository = studentRepository;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<StudentAnswerShowDto>>> Handle(GetStudentAnswersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await studentRepository
                    .GetStudentAnswers(request.UserId,request.TestId).ToListAsync();
                return new ContentResponse<List<StudentAnswerShowDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<StudentAnswerShowDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
