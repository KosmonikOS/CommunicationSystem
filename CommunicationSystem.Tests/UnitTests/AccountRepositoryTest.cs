﻿using AutoFixture;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
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
    public class AccountRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context,logger);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Add_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var expected = new RegistrationDto()
            {
                Email = "test@test.test",
                NickName = "Test",
                Password = "Test"
            };
            var token = "TOKEN";
            var saltPass = FixtureHelper.Fixture.Create<UserSaltPass>();
            //Act
            sut.AddUser(expected,saltPass,token);
            sut.SaveChanges();
            var actual = context.Users.Include(x => x.PassHash).FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.NickName, actual.NickName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(saltPass.PasswordHash, actual.PassHash.PasswordHash);
        }
        [Fact]
        public void ItShould_Update_User_Image()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AccountRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var id = 1;
            var path = "/img/test.png";
            //Act
            var updateResult = sut.UpdateImage(id, path);
            sut.SaveChanges();
            var actual = context.Users.FirstOrDefault();
            //Assert
            Assert.True(updateResult.IsSuccess);
            Assert.Null(updateResult.Message);
            Assert.Equal(path, actual.AccountImage);
        }
        [Fact]
        public void ItShould_Return_NotFound_While_Update_Image()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var id = 1;
            var path = "/img/test.png";
            //Act
            var updateResult = sut.UpdateImage(id, path);
            //Assert
            Assert.False(updateResult.IsSuccess);
            Assert.NotNull(updateResult.Message);
            Assert.Equal(ResponseStatus.NotFound, updateResult.Status);
        }
        [Fact]
        public void ItShould_Find_User_By_Email()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AccountRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var email = "test@test.test";
            //Act
            var actual = sut.GetUserByEmail(email);
            //Assert
            Assert.Equal(email, actual.Email);
        }
        [Fact]
        public void ItShould_Not_Find_User_By_Email()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var email = "test@test.test";
            //Act
            var actual = sut.GetUserByEmail(email);
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public void ItShould_Set_Recover_Password()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            AuthRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var user = context.Users
                .Include(x => x.PassHash).FirstOrDefault();
            var saltPass = new UserSaltPass()
            {
                PasswordHash = "A8@#gRGA3",
                Salt = "NPNZH435FNU3",
            };
            //Act
            var actual = sut.UpdateUserPasswordByEmail(saltPass,user.Email);
            var res = sut.SaveChanges();
            var ac = context.UserSaltPass.FirstOrDefault();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal(saltPass.Salt, user.PassHash.Salt);
            Assert.Equal(saltPass.PasswordHash, user.PassHash.PasswordHash);
        }
        public void ItShould_Return_NotFound_While_Set_Recover_Password()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<AccountRepository>();
            var sut = new AccountRepository(context, logger);
            var saltPass = new UserSaltPass()
            {
                PasswordHash = "A8@#gRGA3",
                Salt = "NPNZH435FNU3",
            };
            //Act
            var actual = sut.UpdateUserPasswordByEmail(saltPass, "test@test.test");
            //Assert
            Assert.True(!actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
    }
}
