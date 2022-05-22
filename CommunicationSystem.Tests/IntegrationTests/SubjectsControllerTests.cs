using AutoFixture;
using CommunicationSystem.Models;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using CommunicationSystem.Tests.Infrastructure.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.IntegrationTests
{
    public class SubjectsControllerTests
    {
        [Fact]
        public async Task ItShould_Return_Subjects_Async()
        {
            //Arrange
            var host = new InMemoryHost("SubjectsControllerTests");
            SubjectDataInitializer.Initialize(host.Context);
            var expected = host.Context.Subjects.ToList();
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("/api/subjects");
            var actual = await HttpHelper.GetDataAsync<List<Subject>>(response);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name, actual[i].Name);
            }
        }
        [Fact]
        public async Task ItShould_Add_Subject_Async()
        {
            //Arrange
            var host = new InMemoryHost("SubjectsControllerTests");
            var expected = new Fixture().Build<Subject>()
                                        .With(s => s.Id, 0).Create();
            //Act
            await host.AuthenticateAsync();
            await host.Client.PostAsync("/api/subjects", HttpHelper.ConvertToHttpContent(expected));
            var actual = host.Context.Subjects.ToList();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.Count());
            Assert.Equal(expected.Name, actual[0].Name);
        }
        [Fact]
        public async Task ItShould_Update_Subject_Async()
        {
            //Arrange
            var host = new InMemoryHost("SubjectsControllerTests");
            SubjectDataInitializer.Initialize(host.Context);
            var expected = host.Context.Subjects.FirstOrDefault();
            expected.Name = "TestSubject";
            //Act
            await host.AuthenticateAsync();
            await host.Client.PostAsync("/api/subjects", HttpHelper.ConvertToHttpContent(expected));
            var actual = host.Context.Subjects.FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }
        [Fact]
        public async Task ItShould_Delete_Subject_Async()
        {
            //Arrange
            var host = new InMemoryHost("SubjectsControllerTests");
            SubjectDataInitializer.Initialize(host.Context);
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.DeleteAsync("/api/subjects/1");
            var actual = host.Context.Subjects.ToList();
            //Assert
            Assert.NotNull(actual);
            Assert.Single(actual);
        }
    }
}
