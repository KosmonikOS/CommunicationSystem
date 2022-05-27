using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Services.Services.Interfaces;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class AuthRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = Mock.Of<IPasswordHashService>();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock, logger);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Confirmed_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var mock = new Mock<IPasswordHashService>();
            mock.Setup(x => x.ComparePasswords(It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
            var expected = new LoginDto()
            {
                Email = "test@test.test",
                Password = "true"
            };
            //Act
            var actual = sut.GetConfirmedUser(expected);
            //Assert
            mock.Verify(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            Assert.True(actual.IsSuccess);
            Assert.Equal(expected.Email, actual.Content.Email);
        }
        [Fact]
        public void ItShould_Return_NotFound_While_GetConfirmedUser()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = new Mock<IPasswordHashService>();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
            var expected = new LoginDto()
            {
                Email = "test@test.test",
                Password = "test"
            };
            //Act
            var actual = sut.GetConfirmedUser(expected);
            //Assert
            mock.Verify(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public void ItShould_Return_BadRequest_While_GetConfirmedUser()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var mock = new Mock<IPasswordHashService>();
            mock.Setup(x => x.ComparePasswords(It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
            var expected = new LoginDto()
            {
                Email = "test@test.test",
                Password = "false"
            };
            //Act
            var actual = sut.GetConfirmedUser(expected);
            //Assert
            mock.Verify(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.BadRequest, actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public void ItShould_Update_Enter_Time()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var mock = Mock.Of<IPasswordHashService>();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock, logger);
            var user = context.Users.AsNoTracking().FirstOrDefault();
            var dto = new UserActivityDto() { Id = user.Id, Activity = UserActivityState.Enter };
            //Act
            var actual = sut.SetTime(dto);
            sut.SaveChanges();
            //Assert
            Assert.NotEqual(user.EnterTime, actual.Entity.EnterTime);
        }
        [Fact]
        public void ItShould_Update_Leave_Time()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var mock = Mock.Of<IPasswordHashService>();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock, logger);
            var user = context.Users.AsNoTracking().FirstOrDefault();
            var dto = new UserActivityDto() { Id = user.Id, Activity = UserActivityState.Leave };
            //Act
            var actual = sut.SetTime(dto);
            sut.SaveChanges();
            //Assert
            Assert.NotEqual(user.LeaveTime, actual.Entity.LeaveTime);
        }
    }
}
