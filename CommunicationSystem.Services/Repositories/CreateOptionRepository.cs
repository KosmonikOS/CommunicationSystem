using CommunicationSystem.Data;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;

namespace CommunicationSystem.Services.Repositories
{
    public class CreateOptionRepository : ICreateOptionRepository
    {
        private readonly CommunicationContext context;

        public CreateOptionRepository(CommunicationContext context)
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
