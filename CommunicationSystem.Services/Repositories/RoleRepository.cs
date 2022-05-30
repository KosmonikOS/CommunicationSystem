using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CommunicationContext context;

        public RoleRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IQueryable<Role> GetRoles()
        {
            return context.Role.AsNoTracking();
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
