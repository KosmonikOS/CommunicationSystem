using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ICreateQuestionRepository :IBaseRepository
    {
        public IQueryable<Question> GetQuestions(Guid id);
        public Task<IResponse> DeleteQuestionAsync(Guid id);
    }
}
