using CommunicationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface ITestRepository
    {
        public Task<List<Test>> GetUserTestsAsync(int id);
        public Task SaveTestAnswerAsync(TestAnswer testAnswer);
    }
}
