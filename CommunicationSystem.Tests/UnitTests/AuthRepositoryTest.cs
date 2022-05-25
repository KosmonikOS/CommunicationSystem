using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using CommunicationSystem.Tests.Infrastructure.Mocks;
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
            var mock = PasswordHashServiceMock.GetMock();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
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
            var mock = PasswordHashServiceMock.GetMock();
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
            Assert.Equal(expected.Email,actual.Content.Email);
        }
        [Fact]
        public void ItShould_Return_NotFound_While_GetConfirmedUser()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var mock = PasswordHashServiceMock.GetMock();
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
            mock.Verify(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),Times.Never);
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound,actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public void ItShould_Return_BadRequest_While_GetConfirmedUser()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var mock = PasswordHashServiceMock.GetMock();
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
            var mock = PasswordHashServiceMock.GetMock();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
            var user = context.Users.AsNoTracking().FirstOrDefault();
            //Act
            var actual = sut.SetTime(user.Id, UserActivityState.Enter);
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
            var mock = PasswordHashServiceMock.GetMock();
            var logger = LoggerHelper.GetLogger<AuthRepository>();
            var sut = new AuthRepository(context, mock.Object, logger);
            var user = context.Users.AsNoTracking().FirstOrDefault();
            //Act
            var actual = sut.SetTime(user.Id, UserActivityState.Leave);
            sut.SaveChanges();
            //Assert
            Assert.NotEqual(user.LeaveTime, actual.Entity.LeaveTime);
        }
    }
}
