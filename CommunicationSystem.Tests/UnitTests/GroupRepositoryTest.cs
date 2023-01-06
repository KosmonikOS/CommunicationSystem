using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class GroupRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new GroupRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Group_By_Id()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupRepositoryDataInitializer.Initialize(context);
            var sut = new GroupRepository(context);
            //Act
            var actual = sut.GetGroups(x => x.Id ==Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"))
                .FirstOrDefault();
            //Assert
            Assert.Equal(actual.Name, "TestName");
            Assert.Equal(actual.GroupImage, "Image");
        }
        [Fact]
        public void ItShould_Add_Group()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new GroupRepository(context);
            var group = new Group()
            {
                Id = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                Name = "Test",
                GroupImage = "Image"
            };
            //Act
            sut.AddGroup(group);
            sut.SaveChanges();
            var actual = context.Groups.AsNoTracking().ToList();
            //Assert
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Update_Group()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupRepositoryDataInitializer.Initialize(context);
            var sut = new GroupRepository(context);
            var group = new Group()
            {
                Id = Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"),
                Name = "Update",
                GroupImage = "UpImage"
            };
            //Act
            sut.UpdateGroup(group);
            sut.SaveChanges();
            var actual = context.Groups.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.Equal(actual.Name, group.Name);
            Assert.Equal(actual.GroupImage, group.GroupImage);
        }
        [Fact]
        public void ItShould_Get_Group_Users()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupRepositoryDataInitializer.Initialize(context);
            var sut = new GroupRepository(context);
            //Act
            var actual = sut.GetGroupMembers(Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"))
                .ToList();
            //Assert
            Assert.Equal(2, actual.Count);
        }
        [Fact]
        public void ItShould_Handel_Group_Members_Update()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupRepositoryDataInitializer.Initialize(context);
            var sut = new GroupRepository(context);
            var members = new List<GroupMemberStateDto>()
            {
                new GroupMemberStateDto(){ UserId = 1,State = DbEntityState.Deleted},
                new GroupMemberStateDto(){ UserId = 2,State = DbEntityState.Added},
                new GroupMemberStateDto(){ UserId = 3,State = DbEntityState.Added},
            };
            //Act
            sut.UpdateGroupMembers(members, Guid.Parse("7049545a-e131-40c4-8227-da9a0d52677f"));
            sut.SaveChanges();
            var actual = context.GroupUser.ToList();
            //Assert
            Assert.Equal(3, actual.Count);
        }
    }
}
