using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using System.Collections.Generic;

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
