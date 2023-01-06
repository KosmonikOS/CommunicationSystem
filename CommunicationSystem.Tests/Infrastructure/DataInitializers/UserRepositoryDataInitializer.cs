using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class UserRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var user = new User()
            {
                Id = 1,
                Role = new Role()
                {
                    RoleId = 1,
                },
                NickName = "User",
                Email = "test@test.test",
                Groups = new List<Group>()
                {
                    new Group()
                    {
                        Id = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                        Name = "Group"
                    }
                }
            };
            var sideUser = new User()
            {
                Id = 2,
                Role = new Role()
                {
                    RoleId = 2,
                },
                Email = "side@side.side"
            };
            var hash = new UserSaltPass()
            {
                Id = 1,
                UserId = user.Id
            };
            var tests = new List<Test>()
            {
                new Test()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test1",
                    CreatorId = user.Id,
                    StudentAnswers = new List<StudentAnswer>()
                    {
                        new StudentAnswer()
                        {
                            Id = 1,
                            Answer = "Answer1",
                            UserId = sideUser.Id,
                        },
                        new StudentAnswer()
                        {
                            Id = 2,
                            Answer = "Answer2",
                            UserId = user.Id,
                        }
                    }
                },
                new Test()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    Students = new List<User>()
                    {
                        user
                    }
                }
            };
            var question = new Question()
            {
                Id = Guid.NewGuid(),
                Text = "Question",
                TestId = tests[0].Id
            };
            var option = new Option()
            {
                Id = Guid.NewGuid(),
                Text = "Option",
                QuestionId = question.Id
            };
            context.Add(user);
            context.Add(sideUser);
            context.Add(hash);
            context.AddRange(tests);
            context.Add(question);
            context.Add(option);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
        public static void InitializePostgreSql(CommunicationContext context)
        {
            var user = FixtureHelper.Fixture.Build<User>()
                .Without(x => x.CreatedTests)
                .Without(x => x.Tests)
                .Without(x => x.StudentAnswers)
                .Without(x => x.Groups)
                .Without(x => x.PassHash)
                .Without(x => x.FromMessages)
                .Without(x => x.ToMessages)
                .With(x => x.EnterTime, DateTime.UtcNow)
                .With(x => x.LeaveTime, DateTime.UtcNow)
                .With(x => x.NickName, "Test")
                .With(x => x.Role, new Role()
                {
                    RoleId = Random.Shared.Next(),
                    Name = "Role"
                })
                .With(x => x.Email, "test@test.test")
                .With(x => x.LastName, "Last")
                .With(x => x.Grade, 11)
                .With(x => x.GradeLetter, "A")
                .With(x => x.FirstName, "First")
                .With(x => x.MiddleName, "Middle").Create();
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
