using AutoFixture;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class MessageRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new MessageRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Messages_Between_Contacts()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Messages");
            MessageRepositoryDataInitializer.InitializePostgreSql(context);
            var sut = new MessageRepository(context);
            //Act
            var actual = sut.GetMessagesBetweenContacts(1, 2, 0).ToList();
            //Assert
            Assert.Equal(2, actual.Count);
            Assert.Equal(DateTime.Parse("2079-06-07T17:12:58+0400"), actual[1].Date);
            Assert.True(actual[0].IsMine);
            Assert.False(actual[1].IsMine);
        }
        [Fact]
        public void ItShould_Get_Group_Messages()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Messages");
            MessageRepositoryDataInitializer.InitializePostgreSql(context);
            var sut = new MessageRepository(context);
            //Act
            var actual = sut.GetGroupMessages(1, Guid.Parse("e2db43c6-5eb4-4fd8-9e4a-0d80605bd817"), 0).ToList();
            //Assert
            Assert.Equal(2, actual.Count);
            Assert.Equal(DateTime.Parse("2078-06-07T17:12:58+0400"), actual[1].Date);
            Assert.Equal("Test", actual[0].NickName);
            Assert.Equal("Image", actual[1].AccountImage);
            Assert.True(actual[1].IsMine);
            Assert.False(actual[0].IsMine);
        }
        [Fact]
        public void ItShould_Add_Message()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new MessageRepository(context);
            var message = FixtureHelper.Fixture.Build<Message>()
                .Without(x => x.From).Without(x => x.To)
                .Without(x => x.Group).Create();
            //Act
            sut.AddMessage(message);
            sut.SaveChanges();
            var actual = context.Messages.ToList();
            //Assert
            Assert.Single(actual);
            Assert.Equal(message.Id, actual[0].Id);
        }
        [Fact]
        public void ItShould_Delet_Message()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            MessageRepositoryDataInitializer.Initialize(context);
            var sut = new MessageRepository(context);
            //Act
            sut.DeleteMessage(1);
            sut.SaveChanges();
            var actual = context.Messages.ToList();
            //Assert
            Assert.Empty(actual);
        }
        [Fact]
        public void ItShould_Update_Message()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            MessageRepositoryDataInitializer.Initialize(context);
            var sut = new MessageRepository(context);
            //Act
            sut.UpdateMessageContent(1, "Updated content");
            sut.SaveChanges();
            var actual = context.Messages.FirstOrDefault();
            //Assert
            Assert.Equal(1, actual.Id);
            Assert.Equal("Updated content", actual.Content);
        }
        [Fact]
        public void ItShould_View_Message()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            MessageRepositoryDataInitializer.Initialize(context);
            var sut = new MessageRepository(context);
            //Act
            sut.ViewMessage(1);
            var actual = context.Messages.FirstOrDefault();
            //Assert
            Assert.Equal(ViewStatus.isViewed, actual.ViewStatus);
        }
    }
}
