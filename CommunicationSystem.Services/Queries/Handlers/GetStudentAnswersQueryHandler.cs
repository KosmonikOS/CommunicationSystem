using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetStudentAnswersQueryHandler : IRequestHandler<GetStudentAnswersQuery, IContentResponse<List<StudentAnswerShowDto>>>
    {
        private readonly IStudentRepository studentRepository;

        public GetStudentAnswersQueryHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        public async Task<IContentResponse<List<StudentAnswerShowDto>>> Handle(GetStudentAnswersQuery request, CancellationToken cancellationToken)
        {
            var dtos = await studentRepository
                .GetStudentAnswers(request.UserId, request.TestId).ToListAsync();
            return new ContentResponse<List<StudentAnswerShowDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
