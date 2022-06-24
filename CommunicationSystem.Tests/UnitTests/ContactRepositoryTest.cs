using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class ContacteRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new ContactRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_User_Contact_List()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Contacts", true);
            ContactRepositoryDataInitializer.InitializePostgreSql(context);
            var sut = new ContactRepository(context);
            //Act
            var actual = sut.GetUserContacts(1).ToList();
            //Assert
            Assert.Equal(2, actual.Count);
            Assert.Equal("User", actual[0].LastMessage);
            Assert.Equal(DateTime.Parse("2079-06-07T17:12:58+0400"), actual[0].LastMessageDate);
            Assert.Equal("Test", actual[0].NickName);
            Assert.Equal(1, actual[0].NotViewedMessages);
            Assert.Equal(2, actual[0].ToId);
            Assert.Equal("Group2", actual[1].LastMessage);
            Assert.Equal(DateTime.Parse("2078-06-07T17:12:58+0400"), actual[1].LastMessageDate);
            Assert.Equal("Group", actual[1].NickName);
            Assert.Equal(0, actual[1].NotViewedMessages);
            Assert.Equal(Guid.Parse("e2db43c6-5eb4-4fd8-9e4a-0d80605bd817"), actual[1].ToGroup);
        }
        [Fact]
        public void ItShould_Get_Contact_User()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Contacts",true);
            ContactRepositoryDataInitializer.InitializePostgreSql(context);
            var sut = new ContactRepository(context);
            //Act
            var actual = sut.GetContact(1);
            //Assert
            Assert.Equal(1, actual.ToId);
            Assert.Equal("Test", actual.NickName);
            Assert.Equal("Image", actual.AccountImage);
            Assert.False(actual.IsGroup);
        }
        [Fact]
        public void ItShould_Get_Contact_Group()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            ContactRepositoryDataInitializer.Initialize(context);
            var sut = new ContactRepository(context);
            //Act
            var actual = sut.GetGroupContact(Guid.Parse("e2db43c6-5eb4-4fd8-9e4a-0d80605bd817"));
            //Assert
            Assert.Equal(Guid.Parse("e2db43c6-5eb4-4fd8-9e4a-0d80605bd817"), actual.ToGroup);
            Assert.Equal("Group", actual.NickName);
            Assert.Equal("Group", actual.AccountImage);
            Assert.True(actual.IsGroup);
        }
    }
}
