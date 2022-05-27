using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetUserAccountQuery : IRequest<IContentResponse<UserAccountDto>>
    {
        public string Email { get; set; }
    }
}
