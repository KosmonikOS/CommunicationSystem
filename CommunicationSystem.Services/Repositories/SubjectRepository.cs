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
                .FirstOrDefault(x => x.Id ==id);
            return context.Remove(subject);
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            return await context.Subject.AsNoTracking().ToListAsync();
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
