using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class SubjectRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var subjects = new List<Subject>()
            {
                FixtureHelper.Fixture.Build<Subject>()
                    .With(x => x.Id,1)
                    .With(x => x.Name,"Test").Create(),
                FixtureHelper.Fixture.Build<Subject>()
                    .With(x => x.Tests,new List<Test>()).Create(),
                FixtureHelper.Fixture.Build<Subject>()
                    .With(x => x.Tests,new List<Test>()).Create(),
            };
            context.AddRange(subjects);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
