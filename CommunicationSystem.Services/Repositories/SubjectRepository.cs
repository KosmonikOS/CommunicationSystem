using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly CommunicationContext db;

        public SubjectRepository(CommunicationContext db)
        {
            this.db = db;
        }
        public async Task DeleteSubjectAsync(int id)
        {
            if (id != 0)
            {
                var subject = db.Subjects.SingleOrDefault(u => u.Id == id);
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = await db.Subjects.AsNoTracking().ToListAsync();
            return subjects;
        }

        public async Task SaveSubjectAsync(Subject subject)
        {
            if (subject != null)
            {
                if (subject.Id > 0)
                {
                    db.Subjects.Update(subject);
                }
                else
                {
                    db.Subjects.Add(subject);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
