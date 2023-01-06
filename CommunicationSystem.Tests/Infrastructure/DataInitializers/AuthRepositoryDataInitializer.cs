using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class AuthRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var hash = FixtureHelper.Fixture.Build<UserSaltPass>()
                .With(x => x.UserId, 1).Create();
            var user = FixtureHelper.Fixture.Build<User>()
                .With(u => u.Email, "test@test.test")
                .With(u => u.PassHash,hash).Create();
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
