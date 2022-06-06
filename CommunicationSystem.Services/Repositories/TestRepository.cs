using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly CommunicationContext context;

        public TestRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IQueryable<Test> GetUserCreateTestsPage(int userId, int role, int page, string search, TestSearchOption searchOption)
        {
            var query = (role != 3
                ? context.Tests.Where(x => x.CreatorId == userId)
                : context.Tests).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (searchOption)
                {
                    case TestSearchOption.Name:
                        query = query.Where(x => EF.Functions.ILike(x.Name, $"%{search}%"));
                        break;
                    case TestSearchOption.Grade:
                        query = query.Where(x => EF.Functions.ILike(x.Grade, $"%{search}%"));
                        break;
                    case TestSearchOption.Subject:
                        query = query.Include(x => x.Subject)
                            .Where(x => EF.Functions.ILike(x.Subject.Name, $"%{search}%"));
                        break;
                }
            }
            query = query.OrderByDescending(x => x.Date);
            if (page > 0)
            {
                query = query.Skip(page * 50);
            }
            return query.Take(50);
        }

        public IQueryable<TestUser> GetUserTestsPage(int userId, int page, string search, TestSearchOption searchOption)
        {
            var query = context.TestUser
                .Include(x => x.Test)
                .Where(x => x.UserId == userId && !x.IsCompleted);                     
            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (searchOption)
                {
                    case TestSearchOption.Name:
                        query = query.Where(x => EF.Functions.ILike(x.Test.Name, $"%{search}%"));
                        break;
                    case TestSearchOption.Subject:
                        query = query.Where(x => EF.Functions.ILike(x.Test.Subject.Name, $"%{search}%"));
                        break;
                }
            }
            query = query.OrderByDescending(x => x.Test.Date);
            if (page > 0)
            {
                query = query.Skip(page * 50);
            }
            return query.Take(50);
        }

        public void AddTest(Test test)
        {
            context.Add(test);
        }

        public void UpdateTest(Test test)
        {
            var testEntry = context.Update(test);
            testEntry.Property(x => x.Date).IsModified = false;
        }

        public async Task<IResponse> DeleteTestAsync(Guid id)
        {
            var test = await context.Tests
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Include(x => x.Students)
                .Include(x => x.StudentAnswers)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (test == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Тест не найден" };
            context.Remove(test);
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
