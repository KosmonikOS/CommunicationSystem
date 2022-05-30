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
                        Id = 1,
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
    }
}
