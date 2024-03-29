﻿using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class StudentRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new StudentRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Answers()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            StudentRepositoryDataInitializer.Initialize(context);
            var sut = new StudentRepository(context);
            var expected = context.Questions.FirstOrDefault();
            //Act
            var actual = sut.GetStudentAnswers(1, Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"))
                .ToList();
            //Assert
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.Image, actual[0].Image);
            Assert.Equal("", actual[0].OpenAnswer);
            Assert.Equal(2, actual[0].Options.Count());
        }
        [Fact]
        public void ItShould_Get_Test_Students()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            StudentRepositoryDataInitializer.Initialize(context);
            var sut = new StudentRepository(context);
            var expected = context.TestUser.FirstOrDefault();
            //Act
            var actual = sut.GetStudents(Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"))
                .FirstOrDefault();
            //Assert
            Assert.Equal(actual.IsCompleted, expected.IsCompleted);
            Assert.Equal(actual.UserId, expected.UserId);
            Assert.Equal(actual.TestId, expected.TestId);
            Assert.Equal(actual.Mark, expected.Mark);
        }
        [Fact]
        public void ItShould_Update_Student_Mark()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            StudentRepositoryDataInitializer.Initialize(context);
            var sut = new StudentRepository(context);
            var dto = new UpdateStudentMarkDto()
            {
                UserId = 1,
                TestId = Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"),
                Mark = 5
            };
            //Act
            var result = sut.UpdateStudentMark(dto);
            var actual = context.TestUser.FirstOrDefault();
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Message);
            Assert.Equal(actual.Mark, 5);
        }
        [Fact]
        public void ItShould_Return_NotFound_While_Update_Student_Mark()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            StudentRepositoryDataInitializer.Initialize(context);
            var sut = new StudentRepository(context);
            var dto = new UpdateStudentMarkDto()
            {
                UserId = 7,
                TestId = Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"),
                Mark = 5
            };
            //Act
            var actual = sut.UpdateStudentMark(dto);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
        [Fact]
        public void ItShold_Handle_Students_List()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            StudentRepositoryDataInitializer.Initialize(context);
            var sut = new StudentRepository(context);
            var students = new List<TestStudentStateDto>()
            {
                new TestStudentStateDto()
                {
                    UserId = 1,
                    State = DbEntityState.Deleted
                },
                new TestStudentStateDto()
                {
                    UserId = 2,
                    State = DbEntityState.Modified,
                    IsCompleted = false
                },
                new TestStudentStateDto()
                {
                    UserId = 3,
                    State = DbEntityState.Added
                }
            };
            //Act
            sut.UpdateTestStudents(students, Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"));
            sut.SaveChanges();
            var testusers = context.TestUser.ToList();
            var answers = context.StudentAnswers.ToList();
            //Assert
            Assert.Equal(2, testusers.Count);
            Assert.False(testusers[0].IsCompleted);
            Assert.Empty(answers);
        }
        [Fact]
        public void ItShould_Add_Student_Answers()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new StudentRepository(context);
            var answer = new StudentFullTestAnswerDto()
            {
                UserId = 1,
                TestId = Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"),
                Questions = new List<StudentQuestionAnswerDto>()
                {
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("41d34938-a4c6-4e67-86f2-e56380c738b6"),
                        Points = 50,
                        QuestionType = QuestionType.Multy,
                        Answers = new List<string>()
                        {
                            "51d34938-a4c6-4e67-86f2-e56380c738b6",
                            "56d34938-a4c6-4e67-86f2-e56380c738b6",
                        }
                    }
                }
            };
            //Act
            sut.AddStudentAnswers(answer);
            sut.SaveChanges();
            var actual = context.StudentAnswers.ToList();
            //Assert
            Assert.Equal(2, actual.Count);
        }
    }
}
