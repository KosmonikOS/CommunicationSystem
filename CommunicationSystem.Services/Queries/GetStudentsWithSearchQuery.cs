using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using MediatR;

namespace CommunicationSystem.Services.Queries
{
    public class GetStudentsWithSearchQuery :IRequest<IContentResponse<List<SearchStudentDto>>>
    {
        public string Search { get; set; }
        public StudentsSearchOption SearchOption { get; set; }
    }
}
