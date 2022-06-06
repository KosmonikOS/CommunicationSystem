using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IOptionRepository :IBaseRepository
    {
        public IQueryable<Option> GetRightOptions(Guid testId);
        public IResponse DeleteOption(Guid id);
    }
}
