using CommunicationSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ITestRepository
    {
        public Task<List<Test>> GetUserTestsAsync(int id);
        public Task SaveTestAnswerAsync(TestAnswer testAnswer);
    }
}
