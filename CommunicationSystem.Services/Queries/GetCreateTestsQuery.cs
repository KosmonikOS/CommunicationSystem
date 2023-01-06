using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetCreateTestsQuery :IRequest<IContentResponse<List<CreateTestShowDto>>>
    {
        public int UserId { get; set; }
        public int Role { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
        public TestPageSearchOption SearchOption { get; set; }
    }
}
