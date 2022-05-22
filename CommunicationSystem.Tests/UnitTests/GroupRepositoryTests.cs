using AutoFixture;
using CommunicationSystem.Models;
using CommunicationSystem.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using CommunicationSystem.Tests.Infrastructure.Mocks.GroupRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class GroupRepositoryTests
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context,mock.Object);
            //Act

            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Return_Group_By_Id() 
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupDataInitializer.Initialize(context);
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = context.Groups.AsNoTracking().FirstOrDefault();
            //Act
            var actual = sut.GetGroup(1);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id,actual.Id);
            Assert.Equal(expected.Name,actual.Name);
            Assert.Equal(expected.GroupImage,actual.GroupImage);   
        }
        [Fact]
        public void ItShould_Return_Group_Members_By_Id()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupDataInitializer.Initialize(context);
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = context.Users.AsNoTracking().ToList();
            //Act
            var actual = sut.GetGroup(1).Users;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Length);
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].accountImage, actual[i].AccountImage);
                Assert.Equal(expected[i].NickName, actual[i].itemName);
            }
        }
        [Fact]
        public void ItShould_Not_Return_Group_By_Id()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupDataInitializer.Initialize(context);
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            //Act
            var actual = sut.GetGroup(2);
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public async Task ItShould_Add_Group_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = new Fixture().Build<Group>()
                                     .With(g => g.Id, 0).Create();
            //Act
            await sut.SaveGroupAsync(expected);
            var actual = context.Groups.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.GroupImage,actual.GroupImage);
        }
        [Fact]
        public async Task ItShould_Add_Group_Members_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = new Fixture().Build<Group>()
                                     .With(g => g.Id, 0)
                                     .With(g => g.Users, new List<GroupUser>()
                                     {
                                         new Fixture().Create<GroupUser>(),
                                         new Fixture().Create<GroupUser>(),
                                         new Fixture().Create<GroupUser>(),
                                     }.ToArray()).Create();
            //Act
            await sut.SaveGroupAsync(expected);
            var actual = context.UsersToGroups.AsNoTracking().ToList();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Users.Count(), actual.Count());
            for (int i = 0; i < expected.Users.Count(); i++)
            {
                Assert.Equal(expected.Users[i].Id, actual[i].UserId);
            }
        }
        [Fact]
        public async Task ItShould_Call_Message_Service_Send_Message()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var group = new Fixture().Build<Group>()
                                     .With(g => g.Id, 0).Create();
            //Act
            await sut.SaveGroupAsync(group);
            //Assert
            mock.Verify(mock => mock.SendMessage(It.IsAny<Message>()),Times.Once());
        }
        [Fact]
        public async void ItShould_Update_Group_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupDataInitializer.Initialize(context);
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = new Fixture().Build<Group>()
                                     .With(g => g.Id, 1).Create();
            expected.Name = "TestName";
            //Act
            await sut.SaveGroupAsync(expected);
            var actual = context.Groups.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }
        [Fact]
        public async void ItShould_Update_Group_Members_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            GroupDataInitializer.Initialize(context);
            var mock = MessageServiceMock.GetMock();
            var sut = new GroupRepository(context, mock.Object);
            var expected = new Fixture().Build<Group>()
                                     .With(g => g.Id, 1)
                                     .With(g => g.Users, new List<GroupUser>()
                                     {
                                         new Fixture().Create<GroupUser>(),
                                         new Fixture().Create<GroupUser>(),
                                         new Fixture().Create<GroupUser>(),
                                     }.ToArray()).Create();
            //Act
            await sut.SaveGroupAsync(expected);
            var actual = context.UsersToGroups.AsNoTracking().ToList();
            //Assert
            Assert.NotNull(actual);
            for (int i = 0; i < expected.Users.Length; i++)
            {
                Assert.Equal(expected.Users[i].Id, actual[i].UserId);
            }
        }
    }
}
