using AutoFixture;
using CommunicationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class AuthDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = new Fixture().Build<User>()
                                    .With(u => u.Id,1)
                                    .With(u => u.Email,"nik.lizard.mobile@gmail.com")
                                    .With(u => u.Password,"MyPassword")
                                    .With(u => u.IsConfirmed,"true").Create();
            context.Users.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
