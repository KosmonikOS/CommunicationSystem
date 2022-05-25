using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        public int SaveChanges();
        public Task<int> SaveChangesAsync();
    }
}
