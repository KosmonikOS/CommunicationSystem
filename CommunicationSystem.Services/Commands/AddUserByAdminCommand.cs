using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class AddUserByAdminCommand :IRequest<IResponse>
    {
        public UserAccountAdminAddDto Dto { get; set; }
    }
}
