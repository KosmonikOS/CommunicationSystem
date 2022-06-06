using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        private readonly CommunicationContext context;

        public OptionRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IResponse DeleteOption(Guid id)
        {
            var option = context.Options.Find(id);
            if (option == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Ответ не найден" };
            context.Remove(option);
            return new BaseResponse(ResponseStatus.Ok);
        }

        public IQueryable<Option> GetRightOptions(Guid testId)
        {
            return context.Options.Include(x => x.Question)
                .Where(x => x.Question.TestId == testId && x.IsRightOption)
                .AsNoTracking();
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
