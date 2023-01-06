using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;
using System.Linq.Expressions;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ISubjectRepository : IBaseRepository
    {
        public IQueryable<Subject> GetSubjectsPage(int page,string search);
        public IQueryable<Subject> GetSubjects();
        public void AddSubject(Subject subject);
        public void UpdateSubject(Subject subject);
        public Task<IResponse> DeleteSubjectAsync(int id);
    }
}
