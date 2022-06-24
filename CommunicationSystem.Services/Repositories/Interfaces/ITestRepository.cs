using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ITestRepository : IBaseRepository
    {
        public IQueryable<Test> GetUserCreateTestsPage(int userId, int role, int page, string search, TestPageSearchOption searchOption);
        public IQueryable<TestUser> GetUserTestsPage(int userId,int page, string search, TestPageSearchOption searchOption);
        public void AddTest(Test test);
        public void UpdateTest(Test test);
        public Task<IResponse> DeleteTestAsync(Guid id);
    }
}
