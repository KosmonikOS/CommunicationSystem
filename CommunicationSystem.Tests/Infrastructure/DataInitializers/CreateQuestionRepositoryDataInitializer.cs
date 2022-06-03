using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class CreateQuestionRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var question = FixtureHelper.FixtureNoNested.Build<Question>()
                .With(x => x.Id, Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"))
                .With(x => x.TestId, Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"))
                .With(x => x.Options,new List<Option>()
                {
                    FixtureHelper.FixtureNoNested.Create<Option>()
                }).Create();
            context.Add(question);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
