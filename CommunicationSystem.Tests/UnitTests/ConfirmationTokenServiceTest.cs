using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Services;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class ConfirmationTokenServiceTest
    {
        [Fact]
        public void IsShould_Create_Inctance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<ConfirmationTokenService>();
            var sut = new ConfirmationTokenService(context,logger);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void IsShould_Create_Token()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<ConfirmationTokenService>();
            var sut = new ConfirmationTokenService(context,logger);
            var email = "test@test.test";
            //Act
            var actual = sut.GenerateToken(email);
            //Assert
            Assert.NotNull(actual);
        }
        [Fact]
        public async Task ItShould_Confirm_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<ConfirmationTokenService>();
            var sut = new ConfirmationTokenService(context,logger);
            var email = "test@test.test";
            var token = Convert.ToHexString(Encoding.ASCII.GetBytes(email)) + "@d@" + Convert.ToHexString(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
            ConfirmDataInitialazer.Initialize(context,token);
            //Act
            var actual = await sut.ConfirmTokenAsync(token);
            var user = context.Users.FirstOrDefault();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal("true",user.IsConfirmed);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_While_Confirm_Token()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<ConfirmationTokenService>();
            var sut = new ConfirmationTokenService(context,logger);
            var token = "TOKEN";
            //Act
            var actual = await sut.ConfirmTokenAsync(token);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.NotNull(actual.Message);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
        }
        [Fact]
        public async Task ItShould_Return_BadRequest_While_Confirm_Token()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<ConfirmationTokenService>();
            var sut = new ConfirmationTokenService(context,logger);
            var email = "test@test.test";
            var token = Convert.ToHexString(Encoding.ASCII.GetBytes(email)) + "@d@" + Convert.ToHexString(Encoding.ASCII.GetBytes((DateTime.Now.AddDays(-3)).ToString()));
            ConfirmDataInitialazer.Initialize(context, token);
            //Act
            var actual = await sut.ConfirmTokenAsync(token);
            var user = context.Users.FirstOrDefault();
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.NotNull(actual.Message);
            Assert.Equal(ResponseStatus.BadRequest, actual.Status);
        }
    }
}
