using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class CreateQuestionRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateQuestionRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Test_Questions()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateQuestionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateQuestionRepository(context);
            //Act
            var actual = sut.GetQuestions(Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"))
                .ToList();
            //Assert
            Assert.Single(actual);
        }
        [Fact]
        public void ItShould_Get_Empty_Test_Questions_List()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateQuestionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateQuestionRepository(context);
            //Act
            var actual = sut.GetQuestions(Guid.Empty)
                .ToList();
            //Assert
            Assert.Empty(actual);
        }
        [Fact]
        public async Task ItShould_Delete_Question()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateQuestionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateQuestionRepository(context);
            //Act
            var actual = await sut.DeleteQuestionAsync(
                Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"));
            sut.SaveChanges();
            var questions = context.Questions.ToList();
            var options = context.Options.ToList();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Empty(questions);
            Assert.Empty(options);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_While_Delete_Question()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateQuestionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateQuestionRepository(context);
            //Act
            var actual = await sut.DeleteQuestionAsync(
                Guid.Parse("51d34738-a4c6-4e67-86f2-e56380c738b6"));
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
    }
}
