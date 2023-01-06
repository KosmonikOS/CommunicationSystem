using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class OptionRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var option = FixtureHelper.FixtureNoNested.Build<Option>()
                .With(x => x.Id, Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"))
                .With(x => x.QuestionId, Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b7"))
                .With(x => x.IsRightOption,true).Create();
            var question = FixtureHelper.FixtureNoNested.Build<Question>()
                .With(x => x.Id, Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b7"))
                .With(x => x.TestId, Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b8"))
                .Create();
            context.Add(option);
            context.Add(question);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
