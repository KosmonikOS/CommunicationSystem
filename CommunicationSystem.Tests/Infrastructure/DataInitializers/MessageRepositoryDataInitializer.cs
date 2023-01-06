using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class MessageRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var message = FixtureHelper.Fixture.Build<Message>()
                .Without(x => x.Group)
                .Without(x => x.To)
                .Without(x => x.From)
                .With(x => x.ViewStatus,ViewStatus.isntViewed)
                .With(x => x.Id, 1).Create();
            context.Add(message);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
        public static void InitializePostgreSql(CommunicationContext context)
        {
            var users = new List<User>()
            {
                FixtureHelper.Fixture.Build<User>()
                .Without(x => x.CreatedTests)
                .Without(x => x.Tests)
                .Without(x => x.StudentAnswers)
                .Without(x => x.Groups)
                .Without(x => x.PassHash)
                .Without(x => x.FromMessages)
                .With(x => x.AccountImage,"Image")
                .Without(x => x.ToMessages)
                .With(x => x.Id,1)
                .With(x => x.EnterTime, DateTime.UtcNow)
                .With(x => x.LeaveTime, DateTime.UtcNow)
                .With(x => x.Role, new Role()
                {
                    RoleId = 4,
                    Name = "Role"
                }).Create(),
                FixtureHelper.Fixture.Build<User>()
                .Without(x => x.CreatedTests)
                .Without(x => x.Tests)
                .Without(x => x.StudentAnswers)
                .Without(x => x.Groups)
                .Without(x => x.PassHash)
                .Without(x => x.FromMessages)
                .Without(x => x.ToMessages)
                .With(x => x.Id,2)
                .With(x => x.NickName,"Test")
                .With(x => x.EnterTime, DateTime.UtcNow)
                .With(x => x.LeaveTime, DateTime.UtcNow)
                .With(x => x.RoleId,4).Create(),
            };
            var group = new Group()
            {
                Id = Guid.Parse("e2db43c6-5eb4-4fd8-9e4a-0d80605bd817"),
                GroupImage = "Group",
                Name = "Group",
                Users = users
            };
            var messages = new List<Message>()
            {
                new Message()
                {
                    FromId = 1,
                    ToId = 2,
                    IsGroup = false,
                    Type = MessageType.Text,
                    ViewStatus = ViewStatus.isntViewed,
                    Content = "User",
                    Date = DateTime.Parse("2023-06-07T17:12:58+0400")
                },
                new Message()
                {
                    FromId = 2,
                    ToId = 1,
                    IsGroup = false,
                    Type = MessageType.Text,
                    ViewStatus = ViewStatus.isntViewed,
                    Content = "User",
                    Date = DateTime.Parse("2079-06-07T17:12:58+0400")
                },
                new Message()
                {
                    FromId = 2,
                    ToGroup = group.Id,
                    IsGroup = true,
                    Type = MessageType.Text,
                    ViewStatus = ViewStatus.isntViewed,
                    Content = "Group1",
                    Date = DateTime.Parse("2069-06-07T17:12:58+0400")
                },
                new Message()
                {
                    FromId = 1,
                    ToGroup = group.Id,
                    IsGroup = true,
                    Type = MessageType.Text,
                    ViewStatus = ViewStatus.isntViewed,
                    Content = "Group2",
                    Date = DateTime.Parse("2078-06-07T17:12:58+0400")
                }
            };
            context.AddRange(users);
            context.Add(group);
            context.AddRange(messages);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
