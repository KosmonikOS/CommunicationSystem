using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly CommunicationContext context;

        public SubjectRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IQueryable<Subject> GetSubjectsPage(int page, string search)
        {
            var query = context.Subject.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search}%"));
            }
            query = query.OrderBy(x => x.Id);
            if (page > 0)
            {
                query = query.Skip(page * 50);
            }
            return query.Take(50);
        }

        public IQueryable<Subject> GetSubjects()
        {
            return context.Subject;
        }

        public void AddSubject(Subject subject)
        {
            context.Add(subject);
        }
        public void UpdateSubject(Subject subject)
        {
            context.Update(subject);
        }

        public async Task<IResponse> DeleteSubjectAsync(int id)
        {
            var subject = await context.Subject
                .Include(x => x.Tests).ThenInclude(x => x.StudentAnswers)
                .Include(x => x.Tests).ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (subject == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Предмет не найден" };
            context.Remove(subject);
            return new BaseResponse(ResponseStatus.Ok);
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
