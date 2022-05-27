using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteSubjectCommand :IRequest<IResponse>
    {
        public int Id { get; set; }
    }
}
