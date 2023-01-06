using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class GroupRepositoryDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var group = new Group()
            {
                Id = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                GroupImage = "Image",
                Name = "TestName"
            };
            var members = new List<GroupUser>()
            {
                new GroupUser()
                {
                    GroupId = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                    UserId = 1
                },
                new GroupUser()
                {
                    GroupId = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                    UserId = 4
                },
            };
            context.Add(group);
            context.AddRange(members);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
