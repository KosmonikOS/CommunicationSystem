using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetRolesQuery :IRequest<IContentResponse<List<Role>>>
    {
    }
}
