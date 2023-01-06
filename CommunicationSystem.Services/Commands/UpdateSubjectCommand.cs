using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateSubjectCommand :IRequest<IResponse>
    {
        public SubjectDto Dto { get; set; }
    }
}
