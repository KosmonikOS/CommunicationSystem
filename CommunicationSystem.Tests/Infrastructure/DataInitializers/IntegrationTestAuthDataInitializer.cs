using AutoFixture;
using CommunicationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class IntegrationTestAuthDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = new Fixture().Build<User>()
                                    .With(u => u.Email, "integration@test.now")
                                    .With(u => u.Password, "MyPassword")
                                    .With(u => u.IsConfirmed, "true").Create();
            context.Users.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
