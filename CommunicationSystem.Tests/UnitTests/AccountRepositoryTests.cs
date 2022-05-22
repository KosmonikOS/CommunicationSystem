using AutoFixture;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class AccountRepositoryTests
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new AccountRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Return_Same_Email()
        {
            //Arrange
            string expected = "nik.lizard.mobile@gmail.com";
            var context = DbContextHelper.CreateInMemoryContext();
            AccountDataInitialazer.Initialize(context);
            var sut = new AccountRepository(context);
            //Act
            var actual = sut.GetUserByEmail(expected)?.Email;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ItShould_Return_Null()
        {
            //Arrange
            var email = "nothing@gmail.com";
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new AccountRepository(context);
            //Act
            var actual = sut.GetUserByEmail(email);
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public async Task ItShould_Add_User_Async()
        {
            //Arrange
            Registration expected = new Fixture().Create<Registration>();
            string token = "Token";
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new AccountRepository(context);
            //Act
            await sut.AddUserAsync(expected,token);
            var actual = context.Users.FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.NickName, actual.NickName);
            Assert.Equal(expected.Password,actual.Password);
        }
        [Fact]
        public async Task ItShould_Update_User_Image_Async()
        {
            //Arrange
            var expected = "/test/path/img.png";
            var context = DbContextHelper.CreateInMemoryContext();
            AccountDataInitialazer.Initialize(context);
            var sut = new AccountRepository(context);
            //Act
            await sut.UpdateImageAsync(1, expected);
            var actual = context.Users.FirstOrDefault().accountImage;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Update_User_Async()
        {
            var context = DbContextHelper.CreateInMemoryContext();
            AccountDataInitialazer.Initialize(context);
            var expected = context.Users.FirstOrDefault();
            var sut = new AccountRepository(context);
            //Act
            expected.Email = "test@test.test";
            await sut.UpdateUserAsync(expected);
            var actual = context.Users.FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id,actual.Id);
        }
    }
}
