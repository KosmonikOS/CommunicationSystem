using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services;
using CommunicationSystem.Tests.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class StudentMarkServiceTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var mock = new Mock<IOptionRepository>();
            var mapper = new Mock<IMapper>();
            var sut = new StudentMarkService(mock.Object, mapper.Object);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public async Task ItShould_Return_Mark_2()
        {
            //Arrange
            var rightOptions = new List<RightOptionDto>() {
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                    Value = "tEst"
                }
            }.AsAsyncQueryable();
            var mock = new Mock<IOptionRepository>();
            mock.Setup(x => x.GetRightOptions(It.IsAny<Guid>())).Returns(new List<Option>().AsQueryable());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo<RightOptionDto>(It.IsAny<IQueryable>(), It.IsAny<object>()))
                .Returns(rightOptions);
            var sut = new StudentMarkService(mock.Object, mapper.Object);
            var dto = new StudentFullTestAnswerDto()
            {
                TestId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                UserId = 1,
                Questions = new List<StudentQuestionAnswerDto>()
                {
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.Single,
                        Answers = new List<string>()
                        {
                            "b6e6b7a9-c4aa-46b8-8761-61010824bffe"
                        }
                    },
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.OpenWithCheck,
                        Answers = new List<string>()
                        {
                            "Test"
                        }
                    }
                }
            };
            //Act
            var actual = await sut.CalculateStudentMarkAsync(dto);
            //Assert
            Assert.Equal(2, actual.Content);
        }
        [Fact]
        public async Task ItShould_Return_Mark_3()
        {
            //Arrange
            var rightOptions = new List<RightOptionDto>() {
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b7e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61110874bffe"),
                    Value ="a7e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                    Value = "test"
                },
            }.AsAsyncQueryable();
            var mock = new Mock<IOptionRepository>();
            mock.Setup(x => x.GetRightOptions(It.IsAny<Guid>())).Returns(new List<Option>().AsQueryable());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo<RightOptionDto>(It.IsAny<IQueryable>(), It.IsAny<object>()))
                .Returns(rightOptions);
            var sut = new StudentMarkService(mock.Object, mapper.Object);
            var dto = new StudentFullTestAnswerDto()
            {
                TestId = Guid.Parse("b5e6b7a9-c1aa-46b8-8761-61010874bffe"),
                UserId = 1,
                Questions = new List<StudentQuestionAnswerDto>()
                {
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.Multy,
                        Answers = new List<string>()
                        {
                            "b7e6b7a9-c4aa-46b8-8761-61010875bffe",
                            "b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                        }
                    },
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.OpenWithCheck,
                        Answers = new List<string>()
                        {
                            "Test"
                        }
                    },
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61110874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.Single,
                        Answers = new List<string>()
                        {
                            "b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                        }
                    },
                }
            };
            //Act
            var actual = await sut.CalculateStudentMarkAsync(dto);
            //Assert
            Assert.Equal(3, actual.Content);
        }
        [Fact]
        public async Task ItShould_Return_Mark_4()
        {
            //Arrange
            var rightOptions = new List<RightOptionDto>() {
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b7e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                    Value = "test"
                },
            }.AsAsyncQueryable();
            var mock = new Mock<IOptionRepository>();
            mock.Setup(x => x.GetRightOptions(It.IsAny<Guid>())).Returns(new List<Option>().AsQueryable());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo<RightOptionDto>(It.IsAny<IQueryable>(), It.IsAny<object>()))
                .Returns(rightOptions);
            var sut = new StudentMarkService(mock.Object, mapper.Object);
            var dto = new StudentFullTestAnswerDto()
            {
                TestId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                UserId = 1,
                Questions = new List<StudentQuestionAnswerDto>()
                {
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.Multy,
                        Answers = new List<string>()
                        {
                            "b6e6b7a9-c4aa-46b8-8761-61010875bffe",
                            "b6e6b7a9-c4aa-96b8-8761-61010824bffe"
                        }
                    },
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.OpenWithCheck,
                        Answers = new List<string>()
                        {
                            "Test"
                        }
                    }
                }
            };
            //Act
            var actual = await sut.CalculateStudentMarkAsync(dto);
            //Assert
            Assert.Equal(4, actual.Content);
        }
        [Fact]
        public async Task ItShould_Return_Mark_5()
        {
            //Arrange
            var rightOptions = new List<RightOptionDto>() {
                new RightOptionDto()
                {
                    QuestionId = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                    Value ="b6e6b7a9-c4aa-46b8-8761-61010875bffe"
                },
            }.AsAsyncQueryable();
            var mock = new Mock<IOptionRepository>();
            mock.Setup(x => x.GetRightOptions(It.IsAny<Guid>())).Returns(new List<Option>().AsQueryable());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo<RightOptionDto>(It.IsAny<IQueryable>(), It.IsAny<object>()))
                .Returns(rightOptions);
            var sut = new StudentMarkService(mock.Object, mapper.Object);
            var dto = new StudentFullTestAnswerDto()
            {
                TestId = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                UserId = 1,
                Questions = new List<StudentQuestionAnswerDto>()
                {
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c4aa-46b8-8761-61010874bffe"),
                        Points = 1,
                        QuestionType = QuestionType.Single,
                        Answers = new List<string>()
                        {
                            "b6e6b7a9-c4aa-46b8-8761-61010875bffe"

                        }
                    },
                    new StudentQuestionAnswerDto()
                    {
                        Id = Guid.Parse("b6e6b7a9-c1aa-46b8-8761-61010874bffe"),
                        Points = 100,
                        QuestionType = QuestionType.Open,
                        Answers = new List<string>()
                        {
                            "Hello"
                        }
                    }
                }
            };
            //Act
            var actual = await sut.CalculateStudentMarkAsync(dto);
            //Assert
            Assert.Equal(5, actual.Content);
        }
    }
}
