using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class RoleRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var roles = new List<Role>()
            {
                new Role(){RoleId = 1},
                new Role(){RoleId = 2},
                new Role(){RoleId = 3},
            };
            context.AddRange(roles);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
