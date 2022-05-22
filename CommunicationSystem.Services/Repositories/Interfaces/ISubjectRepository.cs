using CommunicationSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        public Task<List<Subject>> GetSubjectsAsync();
        public Task SaveSubjectAsync(Subject subject);
        public Task DeleteSubjectAsync(int id);
    }
}
