using AutoFixture;
using CommunicationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class SubjectDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var subjects = new List<Subject>()
            {
                new Fixture().Build<Subject>()
                             .With(s => s.Id,1).Create(),
                new Fixture().Create<Subject>()
            };
            context.Subjects.AddRange(subjects);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
