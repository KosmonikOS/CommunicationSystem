using CommunicationSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace CommunicationSystem.Tests.Infrastructure.Helpers
{
    internal static class DbContextHelper 
    {
        public static CommunicationContext CreateInMemoryContext()
        {
            var builder = new DbContextOptionsBuilder<CommunicationContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = builder.Options;
            var context = new CommunicationContext(options);
            return context;
        }

    }
}
