
using CommunicationSystem.Services.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class PasswordHashServiceTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var sut = new PasswordHashService();
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Create_PasswordHash()
        {
            //Arrange
            var sut = new PasswordHashService();
            var password = "test";
            //Act
            var actual = sut.GenerateSaltPass(password);
            //Assert
            Assert.NotNull(actual.PasswordHash);
            Assert.NotNull(actual.Salt);
        }
        [Fact]
        public void IsShould_Equal_Password_And_Hash()
        {
            //Arrange
            var sut = new PasswordHashService();
            var password = "test";
            var salt = "salt";
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            //Act
            var actual = sut.ComparePasswords(hash, salt, password);
            //Assert
            Assert.True(actual);
        }
        [Fact]
        public void IsShould_Not_Equal_Password_And_Hash()
        {
            //Arrange
            var sut = new PasswordHashService();
            var password = "test";
            var salt = "salt";
            var hash = "hash";
            //Act
            var actual = sut.ComparePasswords(hash, salt, password);
            //Assert
            Assert.False(actual);
        }
    }
}
