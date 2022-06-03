using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class CreateQuestionRepository : ICreateQuestionRepository
    {
        private readonly CommunicationContext context;

        public CreateQuestionRepository(CommunicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Question> GetQuestions(Guid id)
        {
            return context.Questions.AsNoTracking()
                .Where(x => x.TestId == id);
        }

        public async Task<IResponse> DeleteQuestionAsync(Guid id)
        {
            var question = await context.Questions
                .Include(x => x.Options)
                .Include(x => x.StudentAnswers)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Вопрос не найден" };
            context.Remove(question);
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
