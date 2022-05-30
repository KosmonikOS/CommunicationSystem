using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Commands
{
    public class UpdateUserByAdminCommand :IRequest<IResponse>
    {
        public UserAccountAdminUpdateDto Dto { get; set; }
    }
}
