using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IRoleRepository :IBaseRepository
    {
        public IQueryable<Role> GetRoles();
    }
}
