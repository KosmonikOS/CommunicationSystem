using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateStudentMarkCommand :IRequest<IResponse>
    {
        public UpdateStudentMarkDto Dto { get; set; }
    }
}
