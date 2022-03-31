using AutoFixture;
using CommunicationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class AccountDataInitialazer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = new Fixture().Build<User>()
                                    .With(u => u.Email ,"nik.lizard.mobile@gmail.com")
                                    .With(u => u.Id,1).Create();
            context.Users.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
