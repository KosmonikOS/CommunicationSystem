using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CommunicationSystem.Services.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly CommunicationContext context;

        public SubjectRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public EntityEntry<Subject> AddSubject(Subject subject)
        {
            return context.Add(subject);
        }

        public EntityEntry<Subject> DeleteSubject(int id)
        {
            var subject = context.Subject
                .Include(x => x.Tests).ThenInclude(x => x.StudentAnswers)
                .Include(x => x.Tests).ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefault(x => x.Id == id);
            return context.Remove(subject);
        }

        public IQueryable<Subject> GetSubjectsPage(int page,string search)
        {
            var query = context.Subject.AsNoTracking();
            if(!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => EF.Functions.ILike(x.Name,$"%{search}%"));
            }
            if(page > 0)
            {
                query = query.Skip(page * 50);
            }
            return query.Take(50);
        }

        public EntityEntry<Subject> UpdateSubject(Subject subject)
        {
            return context.Update(subject);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

    }
}
