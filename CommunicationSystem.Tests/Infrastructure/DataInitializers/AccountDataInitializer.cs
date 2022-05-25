using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class AccountDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = FixtureHelper.Fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Email, "test@test.test").Create();
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
