using CommunicationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ISubjectRepository : IBaseRepository
    {
        public IQueryable<Subject> GetSubjectsPage(int page,string search);
        public EntityEntry<Subject> AddSubject(Subject subject);
        public EntityEntry<Subject> UpdateSubject(Subject subject);
        public EntityEntry<Subject> DeleteSubject(int id);
    }
}
