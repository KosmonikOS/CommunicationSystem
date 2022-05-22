using CommunicationSystem.Models;
using CommunicationSystem.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class AuthRepositoryTests
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new AuthRepository(context);
            //Act

            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Return_Confirmed_User()
        {
            //Arrange
            var expected = new Login()
            {
                Email = "nik.lizard.mobile@gmail.com",
                Password = "MyPassword"
            };
            var context = DbContextHelper.CreateInMemoryContext();
            AuthDataInitializer.Initialize(context);
            var sut = new AuthRepository(context);
            //Act
            var actual = sut.GetConfirmedUser(expected);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Password, actual.Password);
        }
        [Fact]
        public void ItShould_Not_Return_Confirmed_User()
        {
            var expected = new Login()
            {
                Email = "nothing@gmail.com",
                Password = "Nothing"
            };
            var context = DbContextHelper.CreateInMemoryContext();
            AuthDataInitializer.Initialize(context);
            var sut = new AuthRepository(context);
            //Act
            var actual = sut.GetConfirmedUser(expected);
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public async Task ItShould_Update_Enter_Time_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthDataInitializer.Initialize(context);
            var expected = context.Users.FirstOrDefault()?.EnterTime;
            var sut = new AuthRepository(context);
            //Act
            await sut.SetTimeAsync(1, "enter");
            var actual = context.Users.FirstOrDefault()?.EnterTime;
            //Assert
            Assert.NotNull(actual);
            Assert.NotEqual(expected, actual);
        }
        [Fact]
        public async Task ItShould_Update_Leave_Time_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthDataInitializer.Initialize(context);
            var expected = context.Users.FirstOrDefault()?.LeaveTime;
            var sut = new AuthRepository(context);
            //Act
            await sut.SetTimeAsync(1, "leave");
            var actual = context.Users.FirstOrDefault()?.LeaveTime;
            //Assert
            Assert.NotNull(actual);
            Assert.NotEqual(expected, actual);
        }
    }
}
