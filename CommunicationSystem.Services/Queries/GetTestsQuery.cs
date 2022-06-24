using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetTestsQuery :IRequest<IContentResponse<List<TestShowDto>>>
    {
        public int UserId { get; set; }
        public int Page { get; set; }
        public string Searh { get; set; }
        public TestPageSearchOption SearchOption { get; set; }
    }
}
