using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using CommunicationSystem.Tests.Infrastructure.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.IntegrationTests
{
    public class CreateTestsControllerTests
    {
        [Fact]
        public async Task ItShould_Return_Tests_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = host.Context.Tests.AsNoTracking().ToList();
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/1");
            var actual = await HttpHelper.GetDataAsync<List<Test>>(response);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Date, actual[i].Date);
            }
        }
        [Fact]
        public async Task ItShould_Return_Questions_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = host.Context.Questions.AsNoTracking().ToList();
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/1");
            var actual = (await HttpHelper.GetDataAsync<List<Test>>(response))[0].QuestionsList;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Text, actual[i].Text);
                Assert.Equal(expected[i].QuestionType, actual[i].QuestionType);
            }
        }
        [Fact]
        public async Task ItShould_Return_Options_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = host.Context.Questions.AsNoTracking().ToList();
            expected.ForEach(question =>
            {
                question.Options = host.Context.Options.Where(o => o.QuestionId == question.Id).AsNoTracking().ToList();
            });
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/1");
            var actual = (await HttpHelper.GetDataAsync<List<Test>>(response))[0].QuestionsList;
            //Assert
            Assert.NotNull(actual);
            for (int i = 0; i < expected.Count(); i++)
            {
                for (int j = 0; j < expected[i].Options.Count(); j++)
                {
                    Assert.Equal(expected[i].Options[j].Id, actual[i].Options[j].Id);
                    Assert.Equal(expected[i].Options[j].QuestionId, actual[i].Options[j].QuestionId);
                    Assert.Equal(expected[i].Options[j].IsRightOption, actual[i].Options[j].IsRightOption);
                    Assert.Equal(expected[i].Options[j].Text, actual[i].Options[j].Text);
                }
            }
        }
        [Fact]
        public async Task ItShould_Not_Return_Creator_Tests_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/1");
            var actual = await HttpHelper.GetDataAsync<List<Test>>(response);
            //Assert
            Assert.Null(actual);
        }
        [Fact]
        public async Task ItShould_Return_Students_By_Param_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = host.Context.Users.AsNoTracking().Skip(2).ToList();
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/getusers/test");
            var actual = await HttpHelper.GetDataAsync<List<UsersToTests>>(response);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].UserId);
                Assert.Equal(expected[i].GetFullName, actual[i].Name);
                Assert.Equal(expected[i].GetFullGrade, actual[i].Grade);
            }
        }
        [Fact]
        public async Task ItShould_Not_Return_Students_By_Param_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/getusers/test");
            var actual = await HttpHelper.GetDataAsync<List<UsersToTests>>(response);
            //Assert
            Assert.Empty(actual);
        }
        [Fact]
        public async Task ItShould_Return_Student_Answers_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = host.Context.Questions.AsNoTracking().Select(q => host.Context.StudentAnswers.FirstOrDefault(a => a.QuestionId == q.Id).Answer ?? "").ToList();
            //Act
            await host.AuthenticateAsync();
            var response = await host.Client.GetAsync("api/createtests/getanswers/1/1");
            var actual = await HttpHelper.GetDataAsync<List<Question>>(response);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i], actual[i].OpenAnswer);
            }
        }
        [Fact]
        public async Task ItShould_Update_Student_Mark_Async()
        {
            //Arrange
            var host = new InMemoryHost("CreateTestsControllerTests");
            CreateTestDataInitialzier.Initialize(host.Context);
            var expected = 5;
            //Act
            await host.AuthenticateAsync();
            await host.Client.PutAsync($"/api/createtests/1/1/{expected}",null);
            var actual = host.Context.UsersToTests.AsNoTracking().FirstOrDefault().Mark;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
    }
}
