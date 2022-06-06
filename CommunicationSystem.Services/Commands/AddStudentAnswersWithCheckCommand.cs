using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class AddStudentAnswersWithCheckCommand :IRequest<IResponse>
    {
        public StudentFullTestAnswerDto Dto { get; set; }
    }
}
