using CommunicationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface ICreateTestRepository
    {
        public Task<List<Test>> GetUsersTestsAsync(int id);
        public Task<List<Question>> GetUsersAnswersAsync(int id,int testId);
        public Task<List<UsersToTests>> GetStudentsByParamAsync(string param);
        public Task SaveTestAsync(Test test);
        public Task DeleteTestEntity(string type, int id);
        public Task DeleteTestWithoutSavingAsync(int id);
        public Task DeleteQuestionWithoutSavingAsync(int id);
        public void DeleteOptionWithoutSaving(int id);
        public Task UpdateStudentMarkAsync(int studentId, int testId, int mark);
    }
}
