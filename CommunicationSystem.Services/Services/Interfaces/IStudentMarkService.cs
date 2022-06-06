using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IStudentMarkService
    {
        public Task<IContentResponse<int>> CalculateStudentMarkAsync(StudentFullTestAnswerDto dto);
    }
}
