using AutoFixture;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class SubjectRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new SubjectRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Subjects()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = sut.GetSubjectsPage(0,"").ToList();
            //Assert
            Assert.Equal(3, actual.Count);
            Assert.Equal(1, actual[0].Id);
        }
        [Fact]
        public void ItShould_Get_Subject_By_Search()
        {
            //Arrange
            var context = DbContextHelper.CreatePostgreSqlContext("Subjects");
            SubjectRepositoryDataInitializer.InitializePostgreSql(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = sut.GetSubjectsPage(0, "test").ToList();
            //Assert
            Assert.Single(actual);
        }
        public void ItShould_Not_Get_Subjects_By_Page()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = sut.GetSubjectsPage(100, null).ToList();
            //Assert
            Assert.Empty(actual);
        }
        [Fact]
        public async Task ItShould_Add_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new SubjectRepository(context);
            var subject = FixtureHelper.Fixture.Create<Subject>();
            //Act
            sut.AddSubject(subject) ;
            sut.SaveChanges();
            var actual = context.Subject.FirstOrDefault();
            //Assert
            Assert.Equal(subject.Id,actual.Id);
            Assert.Equal(subject.Name, actual.Name);
        }
        [Fact]
        public async Task ItShould_Update_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            var subject = new Subject()
            {
                Id = 1,
                Name = "Test"
            };
            //Act
            sut.UpdateSubject(subject);
            sut.SaveChanges();
            var actual = context.Subject.FirstOrDefault();
            //Assert
            Assert.Equal(subject.Id, actual.Id);
            Assert.Equal(subject.Name, actual.Name);
        }
        [Fact]
        public async Task ItShould_Delete_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = await sut.DeleteSubjectAsync(1);
            sut.SaveChanges();
            var subjects = context.Subject.ToList();
            var tests = context.Tests.ToList();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Equal(2, subjects.Count);
            Assert.Equal(0, tests.Count);
        }
        [Fact]
        public async Task ItShould_Return_NotFound_While_Delete_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = await sut.DeleteSubjectAsync(5);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
    }
}
