using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class TestRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var tests = new List<Test>()
            {
                FixtureHelper.FixtureNoNested.Build<Test>()
                .With(x => x.CreatorId, 1)
                .With(x => x.Id, Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6")).Create(),
                FixtureHelper.FixtureNoNested.Create<Test>()
            };
            var question = FixtureHelper.FixtureNoNested.Build<Question>()
                .With(x => x.TestId, tests[0].Id)
                .With(x => x.QuestionType, QuestionType.Single)
                .With(x => x.Options, new List<Option>()
                {
                    new Option() {
                        Id = Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"),
                        IsRightOption = true,
                    },
                    new Option() {
                        Id = Guid.Parse("61d34938-a4c6-4e67-86f2-e56380c738b6"),
                        IsRightOption = false,
                    }
                })
                .With(x => x.StudentAnswers, new List<StudentAnswer>()
                {
                    new StudentAnswer()
                    {
                        Id = 1,
                        Answer = "51d34938-a4c6-4e67-86f2-e56380c738b6",
                        QuestionId = Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"),
                        TestId = Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"),
                        UserId = 1
                    }
                }).Create();
            var teststousers = FixtureHelper.FixtureNoNested.Build<TestUser>()
                .With(x => x.TestId, Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"))
                .With(x => x.UserId, 1).Create();
            var users = FixtureHelper.FixtureNoNested.CreateMany<User>(3);
            context.AddRange(tests);
            context.Add(question);
            context.Add(teststousers);
            context.AddRange(users);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
