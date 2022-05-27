using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class JwtServiceDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = FixtureHelper.Fixture.Build<User>()
                .With(u => u.Id, 20)
                .With(u => u.RefreshToken,"edhfi$hsi@nd8i").Create();
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
