using CommunicationSystem.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace CommunicationSystem.Tests.Infrastructure.Helpers
{
    internal static class DbContextHelper
    {
        public static CommunicationContext CreateInMemoryContext()
        {
            var builder = new DbContextOptionsBuilder<CommunicationContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString(), x => x.EnableNullChecks(false))
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = builder.Options;
            var context = new CommunicationContext(options);
            context.Database.EnsureCreated();
            return context;
        }
        public static CommunicationContext CreatePostgreSqlContext(string databaseName, bool migrate = false)
        {
            var config = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly()).Build();
            var connectionString = config.GetConnectionString("TestDb") + "TestDb" + databaseName;
            var builder = new DbContextOptionsBuilder<CommunicationContext>()
                    .UseNpgsql(connectionString);
            var options = builder.Options;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var context = new CommunicationContext(options);
            if (context.Database.CanConnect())
                DbWipeHelper.WipeDbData(context);
            if (migrate)
                context.Database.Migrate();
            else
                context.Database.EnsureCreated();
            context.Database.SetCommandTimeout(600);
            return context;
        }
    }
}
