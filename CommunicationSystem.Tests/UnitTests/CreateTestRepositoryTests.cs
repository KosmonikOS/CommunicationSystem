using AutoFixture;
using CommunicationSystem.Models;
using CommunicationSystem.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class CreateTestRepositoryTests
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateTestRepository(context);
            //Act

            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public async Task ItShould_Return_Tests_By_Creator_Id_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Tests.AsNoTracking().ToList();
            //Act
            var actual = await sut.GetUsersTestsAsync(1);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }
        [Fact]
        public async Task ItShould_Return_Questions_For_Test()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var test = context.Tests.FirstOrDefault();
            var expected = context.Questions.Where(q => q.TestId == test.Id).AsNoTracking().ToList();
            //Act
            var actual = (await sut.GetUsersTestsAsync(1)).FirstOrDefault()?.QuestionsList;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }
        [Fact]
        public async Task ItShould_Return_Options_For_Question()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var test = context.Tests.AsNoTracking().FirstOrDefault();
            var question = context.Questions.AsNoTracking().Where(q => q.TestId == test.Id).FirstOrDefault();
            var expected = context.Options.AsNoTracking().Where(o => o.QuestionId == question.Id).ToList();
            //Act
            var actual = (await sut.GetUsersTestsAsync(1)).FirstOrDefault()?.QuestionsList[0].Options;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }
        [Fact]
        public async Task ItShould_Return_User_Answers_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Questions.AsNoTracking().Select(q => context.StudentAnswers.FirstOrDefault(a => a.QuestionId == q.Id).Answer ?? "").ToList();
            //Act
            var actual = (await sut.GetUsersAnswersAsync(1, 1));
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i], actual[i].OpenAnswer);
            }
        }
        [Fact]
        public async Task ItShould_Return_Students_By_Param_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Users.Skip(1).AsNoTracking().ToList();
            //Act
            var actual = await sut.GetStudentsByParamAsync("Test");
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, actual[i].UserId);
                Assert.Equal(expected[i].GetFullGrade, actual[i].Grade);
                Assert.Equal(expected[i].GetFullName, actual[i].Name);
            }
        }
        [Fact]
        public async Task ItShould_Add_Test_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateTestRepository(context);
            var expected = 1;
            var test = new Fixture().Build<Test>()
                                        .With(t => t.Id, 0)
                                        .With(t => t.Students, new List<UsersToTests>())
                                        .With(t => t.QuestionsList, new List<Question>()
                                        {
                                            new Fixture().Build<Question>()
                                                         .With(q => q.Id,0)
                                                         .With(q => q.Options,new List<Option>()
                                                         {
                                                             new Fixture().Build<Option>()
                                                             .With(o => o.Id,0).Create()
                                                         })
                                                         .Create()
                                        }).Create();
            //Act
            await sut.SaveTestAsync(test);
            var actual = context.Tests;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual.Count());
        }
        [Fact]
        public async Task ItShould_Add_Questions_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateTestRepository(context);
            var expected = 1;
            var test = new Fixture().Build<Test>()
                                        .With(t => t.Id, 0)
                                        .With(t => t.Students, new List<UsersToTests>())
                                        .With(t => t.QuestionsList, new List<Question>()
                                        {
                                            new Fixture().Build<Question>()
                                                         .With(q => q.Id,0)
                                                         .With(q => q.Options,new List<Option>()
                                                         {
                                                             new Fixture().Build<Option>()
                                                             .With(o => o.Id,0).Create()
                                                         })
                                                         .Create()
                                        }).Create();
            //Act
            await sut.SaveTestAsync(test);
            var actual = context.Questions.Count();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Add_Options_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateTestRepository(context);
            var expected = 1;
            var test = new Fixture().Build<Test>()
                                        .With(t => t.Id, 0)
                                        .With(t => t.Students, new List<UsersToTests>())
                                        .With(t => t.QuestionsList, new List<Question>()
                                        {
                                            new Fixture().Build<Question>()
                                                         .With(q => q.Id,0)
                                                         .With(q => q.Options,new List<Option>()
                                                         {
                                                             new Fixture().Build<Option>()
                                                             .With(o => o.Id,0).Create()
                                                         })
                                                         .Create()
                                        }).Create();
            //Act
            await sut.SaveTestAsync(test);
            var actual = context.Options.Count();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Update_Student_Mark_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = 5;
            //Act
            await sut.UpdateStudentMarkAsync(1, 1, expected);
            var actual = context.UsersToTests.FirstOrDefault().Mark;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Delete_Test_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = 0;
            //Act
            await sut.DeleteTestWithoutSavingAsync(1);
            context.SaveChanges();
            var actual = context.Tests.Count();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Delete_Questions_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = 0;
            //Act
            await sut.DeleteTestWithoutSavingAsync(1);
            context.SaveChanges();
            var actual = context.Questions.AsNoTracking().Count();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Delete_Options_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = 0;
            //Act
            await sut.DeleteTestWithoutSavingAsync(1);
            context.SaveChanges();
            var actual = context.Options.Count();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task ItShould_Update_Test_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Tests.AsNoTracking().FirstOrDefault();
            //Act
            expected.Subject = 2;
            await sut.SaveTestAsync(expected);
            var actual = context.Tests.AsNoTracking().FirstOrDefault();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Subject, actual.Subject);
        }
        [Fact]
        public async Task ItShould_Update_Questions_Async()
        {
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Tests.AsNoTracking().FirstOrDefault();
            expected.QuestionsList = context.Questions.Where(q => q.TestId == expected.Id).AsNoTracking().ToList();
            expected.QuestionsList[0].Points = 100;
            expected.QuestionsList[1].Points = 200;
            //Act
            await sut.SaveTestAsync(expected);
            var actual = context.Questions.AsNoTracking().ToList();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.QuestionsList.Count(), actual.Count());
            for (int i = 0; i < expected.QuestionsList.Count(); i++)
            {
                Assert.Equal(expected.QuestionsList[i].Id, actual[i].Id);
                Assert.Equal(expected.QuestionsList[i].Points, actual[i].Points);
            }
        }
        [Fact]
        public async Task ItShould_Update_Options_Async()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateTestDataInitialzier.Initialize(context);
            var sut = new CreateTestRepository(context);
            var expected = context.Tests.FirstOrDefault();
            expected.QuestionsList.Add(context.Questions.FirstOrDefault());
            expected.QuestionsList[0].Options = context.Options.Where(o => o.QuestionId == 1).AsNoTracking().ToList();
            expected.QuestionsList[0].Options.ForEach(o => o.Text = Guid.NewGuid().ToString());
            //Act
            await sut.SaveTestAsync(expected);
            var actual = context.Options.AsNoTracking().Where(o => o.QuestionId == 1).ToList();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.QuestionsList[0].Options.Count(), actual.Count());
            for (int i = 0; i < expected.QuestionsList[0].Options.Count(); i++)
            {
                Assert.Equal(expected.QuestionsList[0].Options[i].Id, actual[i].Id);
                Assert.Equal(expected.QuestionsList[0].Options[i].Text, actual[i].Text);
            }
        }
    }
}

