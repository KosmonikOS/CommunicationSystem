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
        public async Task ItShould_Get_Subjects()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = await sut.GetSubjectsAsync();
            //Assert
            Assert.Equal(3, actual.Count);
            Assert.Equal(1, actual[0].Id);
        }
        [Fact]
        public async Task ItShould_Add_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new SubjectRepository(context);
            var subject = FixtureHelper.Fixture.Create<Subject>();
            //Act
            var actual = sut.AddSubject(subject) ;
            sut.SaveChanges();
            //Assert
            Assert.Equal(subject.Id,actual.Entity.Id);
            Assert.Equal(subject.Name, actual.Entity.Name);
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
            var actual = sut.UpdateSubject(subject);
            sut.SaveChanges();
            //Assert
            Assert.Equal(subject.Id, actual.Entity.Id);
            Assert.Equal(subject.Name, actual.Entity.Name);
        }
        [Fact]
        public async Task ItShould_Delete_Subject()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            SubjectRepositoryDataInitializer.Initialize(context);
            var sut = new SubjectRepository(context);
            //Act
            var actual = sut.DeleteSubject(1);
            sut.SaveChanges();
            var subjects = context.Subject.ToList();
            var tests = context.Tests.ToList();
            //Assert
            Assert.Equal(2, subjects.Count);
            Assert.Equal(0, tests.Count);
        }
    }
}
