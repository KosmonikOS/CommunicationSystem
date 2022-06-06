using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class TestRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new TestRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Teacher_Tests()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            var test = context.Tests.FirstOrDefault();
            //Act
            var actual = sut.GetUserCreateTestsPage(1, 2, 0, "", TestSearchOption.Name) //2 is teacher role
                .ToList();
            //Assert
            Assert.Single(actual);
            Assert.Equal(actual[0].Id, test.Id);
            Assert.Equal(actual[0].Name, test.Name);
            Assert.Equal(actual[0].CreatorId, test.CreatorId);
        }
        [Fact]
        public void ItShould_Get_Admin_Tests()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            //Act
            var actual = sut.GetUserCreateTestsPage(1, 3, 0, "", TestSearchOption.Name) // 3 is admin role
                .ToList();
            //Assert
            Assert.Equal(2,actual.Count);
        }
        [Fact]
        public void ItShould_Get_Student_Tests()
        {
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            //Act
            var actual = sut.GetUserTestsPage(1, 0, "", TestSearchOption.Name)
                .ToList();
            //Assert
            Assert.Equal(1, actual.Count);
        }
        [Fact]
        public void ItShould_Get_Empty_Tests_List()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            //Act
            var actual = sut.GetUserCreateTestsPage(5, 2, 0, "", TestSearchOption.Name)
                .ToList();
            //Assert
            Assert.Empty(actual);
        }
        [Fact]
        public void ItShould_Add_Test()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new TestRepository(context);
            var test = new Test()
            {
                Id = Guid.NewGuid(),
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        Id = Guid.NewGuid(),
                        Options = new List<Option>()
                        {
                            new Option()
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }
                }
            };
            //Act
            sut.AddTest(test);
            sut.SaveChanges();
            var tests = context.Tests.ToList();
            var questions = context.Questions.ToList();
            var options = context.Options.ToList();
            //Assert
            Assert.Single(tests);
            Assert.Single(questions);
            Assert.Single(options);
        }
        [Fact]
        public void ItShould_Update_Test()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            var expected = context.Tests.AsNoTracking().FirstOrDefault();
            //Act
            expected.Name = "Test";
            expected.SubjectId = 1;
            sut.UpdateTest(expected);
            sut.SaveChanges();
            var actual = context.Tests.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.SubjectId,actual.SubjectId);
            Assert.Equal(expected.CreatorId,actual.CreatorId);
        }
        [Fact]
        public async Task ItShould_Delete_Test()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            //Act
            var actual = await sut.DeleteTestAsync(Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"));
            sut.SaveChanges();
            var tests = context.Tests.ToList();
            var questions = context.Questions.ToList();
            var options = context.Options.ToList();
            var answers = context.StudentAnswers.ToList();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Single(tests);
            Assert.Empty(questions);
            Assert.Empty(options);
            Assert.Empty(answers);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_Delete_Test()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            TestRepositoryDataInitializer.Initialize(context);
            var sut = new TestRepository(context);
            //Act
            var actual = await sut.DeleteTestAsync(Guid.Parse("41d34958-a4c6-4e67-86f2-e56380c738b6"));
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
    }
}
