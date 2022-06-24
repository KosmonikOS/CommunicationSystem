using AutoFixture;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class UserRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context,logger);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Add_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
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
        public void ItShould_Add_User_By_Admin()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            var expected = new User()
            {
                Email = "test@test.test",
                NickName = "Test",
                FirstName = "Test",
                LastName = "Test",
                MiddleName = "Test",
                Grade = 1,
                GradeLetter = "T",
                RoleId = 1,
            };
            //Act
            sut.AddUser(expected);
            sut.SaveChanges();
            var actual = context.Users.FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.NickName, actual.NickName);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Grade, actual.Grade);
            Assert.Equal(expected.RoleId, actual.RoleId);
        }
        [Fact]
        public void ItShould_Find_User_By_Email()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            var email = "test@test.test";
            //Act
            var actual = sut.GetUsers(x => x.Email == email)
                .FirstOrDefault();
            //Assert
            Assert.Equal(email, actual.Email);
        }
        [Fact]
        public void ItShould_Not_Find_User_By_Email()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            var email = "test@test.test";
            //Act
            var actual = sut.GetUsers(x => x.Email == email)
                .FirstOrDefault();
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public void ItShould_Get_Users_By_Page()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(0,"",UserPageSearchOption.Email)
                .ToList();
            //Arrange
            Assert.Equal(2,actual.Count);
        }
        [Fact]
        public void ItShould_Not_Get_Users_By_Page()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(1, "", UserPageSearchOption.Email)
                .ToList();
            //Arrange
            Assert.Empty(actual);
        }
        [Fact]
        public void ItShould_Not_Get_Users_By_Search_NickName()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(0, "tes", UserPageSearchOption.NickName)
                .ToList();
            //Arrange
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Get_Test_Students_By_Search_Name()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context,logger);
            //Act
            var actual = sut.GetUsersWithSearch("first mid", UserSearchOption.FullName).ToList();
            //Assert
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Get_Test_Students_By_Search_Grade()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersWithSearch("11 a", UserSearchOption.Grade).ToList();
            //Assert
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Not_Get_Users_By_Search_Email()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(0, "test@te", UserPageSearchOption.Email)
                .ToList();
            //Arrange
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Not_Get_Users_By_Search_FullName()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(0, "Last First mi", UserPageSearchOption.FullName)
                .ToList();
            //Arrange
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Not_Get_Users_By_Search_Role()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Users");
            UserRepositoryDataInitializer.InitializePostgreSql(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = sut.GetUsersPage(0, "Ro", UserPageSearchOption.Role)
                .ToList();
            //Arrange
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Set_Recover_Password()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
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
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            var saltPass = new UserSaltPass()
            {
                PasswordHash = "A8@#gRGA3",
                Salt = "NPNZH435FNU3",
            };
            //Act
            var actual = sut.UpdateUserPasswordByEmail(saltPass, "test@test.test");
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public async Task ItShould_Delete_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = await sut.DeleteUserAsync(1);
            sut.SaveChanges();
            var users = context.Users.ToList();
            var questions = context.Questions.ToList();
            var options = context.Options.ToList();
            var hashes = context.UserSaltPass.ToList();
            var tests = context.Tests.ToList();
            var Users = context.Users.ToList();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Single(users);
            Assert.Empty(questions);
            Assert.Empty(options);
            Assert.Empty(hashes);
            Assert.Single(tests);
            Assert.Single(Users);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_While_Delete_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            //Act
            var actual = await sut.DeleteUserAsync(5);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public void ItShould_Update_User()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            UserRepositoryDataInitializer.Initialize(context);
            var logger = LoggerHelper.GetLogger<UserRepository>();
            var sut = new UserRepository(context, logger);
            var expected = context.Users.AsNoTracking().FirstOrDefault();
            //Act
            expected.NickName = "Test";
            expected.AccountImage = "UpdatedImage";
            expected.Email = "test@test.test";
            expected.IsConfirmed = "hh$G*G*$@#*BF";
            sut.UpdateUser(expected,false);
            sut.SaveChanges();
            var actual = context.Users.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.Equal(expected.NickName, actual.NickName);
            Assert.Equal(expected.AccountImage,actual.AccountImage);
            Assert.Equal(expected.FirstName,actual.FirstName);
            Assert.NotEqual(expected.IsConfirmed, actual.IsConfirmed);
        }
    }
}
