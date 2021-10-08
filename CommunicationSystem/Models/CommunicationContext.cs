using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class CommunicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public CommunicationContext(DbContextOptions<CommunicationContext> options): base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
