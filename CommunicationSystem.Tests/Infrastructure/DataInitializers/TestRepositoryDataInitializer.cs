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
                .With(x => x.Date,DateTime.UtcNow)
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
        public static void InitializePostgreSql(CommunicationContext context)
        {
            var test = FixtureHelper.FixtureNoNested.Build<Test>()
                .With(x => x.Name, "111")
                .With(x => x.Date, DateTime.UtcNow)
                .With(x => x.CreatorId, 1)
                .With(x => x.Grade, "11 a")
                .With(x => x.Id, Guid.NewGuid)
                .With(x => x.SubjectId, 1).Create();
            var user = FixtureHelper.Fixture.Build<User>()
                .Without(x => x.CreatedTests)
                .Without(x => x.Tests)
                .Without(x => x.StudentAnswers)
                .Without(x => x.Groups)
                .Without(x => x.PassHash)
                .Without(x => x.FromMessages)
                .Without(x => x.ToMessages)
                .With(x => x.EnterTime, DateTime.Now)
                .With(x => x.LeaveTime, DateTime.Now)
                .With(x => x.Id, 1).Create();
            var subject = FixtureHelper.FixtureNoNested.Build<Subject>()
                .With(x => x.Id, 1)
                .With(x => x.Name, "Subject").Create();
            var testuser = new TestUser()
            {
                UserId = user.Id,
                TestId = test.Id,
                IsCompleted = false,
            };
            context.Add(subject);
            context.Add(user);
            context.Add(test);
            context.Add(testuser);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
