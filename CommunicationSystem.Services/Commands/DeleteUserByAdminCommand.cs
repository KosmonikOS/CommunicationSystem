using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class DeleteUserByAdminCommand :IRequest<IResponse>
    {
        public int Id { get; set; }
    }
}
