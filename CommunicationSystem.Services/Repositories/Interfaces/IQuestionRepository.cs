using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IQuestionRepository :IBaseRepository
    {
        public IQueryable<Question> GetQuestions(Guid id);
        public Task<IResponse> DeleteQuestionAsync(Guid id);
    }
}
