using CommunicationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        public Task<List<Subject>> GetSubjectsAsync();
        public Task SaveSubjectAsync(Subject subject);
        public Task DeleteSubjectAsync(int id);
    }
}
