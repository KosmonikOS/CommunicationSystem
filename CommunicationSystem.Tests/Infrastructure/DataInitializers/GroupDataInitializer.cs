using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class GroupDataInitializer
    {
        public static void Initialize(CommunicationContext context)
        {
            var users = new List<User>()
            {
                new Fixture().Create<User>(),
                new Fixture().Create<User>()
            };
            var group = new Fixture().Build<Group>()
                                     .With(g => g.Users,new GroupUser[0])
                                     .With(g => g.Id,1).Create();
            var utg = new List<UsersToGroups>()
            {
                new Fixture().Build<UsersToGroups>()
                             .With(utg => utg.UserId,users[0].Id)
                             .With(utg => utg.GroupId,group.Id).Create(),
                new Fixture().Build<UsersToGroups>()
                             .With(utg => utg.UserId,users[1].Id)
                             .With(utg => utg.GroupId,group.Id).Create()
            };
            context.Users.AddRange(users);
            context.Groups.Add(group);
            context.UsersToGroups.AddRange(utg);
            context.SaveChanges();
            context.ChangeTracker.Clear();

        }
    }
}
