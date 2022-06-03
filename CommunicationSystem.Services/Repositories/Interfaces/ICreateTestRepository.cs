using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ICreateTestRepository : IBaseRepository
    {
        public IQueryable<Test> GetUserTestsPage(int userId, int role, int page, string search, TestSearchOption searchOption);
        public void AddTest(Test test);
        public void UpdateTest(Test test);
        public Task<IResponse> DeleteTestAsync(Guid id);
    }
}
