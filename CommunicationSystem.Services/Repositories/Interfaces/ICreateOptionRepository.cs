using CommunicationSystem.Services.Infrastructure.Responses;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface ICreateOptionRepository :IBaseRepository
    {
        public IResponse DeleteOption(Guid id);
    }
}
