using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class ConfirmDataInitialazer
    {
        public static void Initialize(CommunicationContext context,string token)
        {
            var user = FixtureHelper.Fixture.Build<User>()
                .With(u => u.IsConfirmed,token).Create();
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
