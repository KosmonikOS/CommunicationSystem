﻿using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class CreateOptionRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new CreateOptionRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Delete_Option()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateOptionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateOptionRepository(context);
            //Act
            var actual = sut.DeleteOption(Guid.Parse("51d34938-a4c6-4e67-86f2-e56380c738b6"));
            sut.SaveChanges();
            var options = context.Options.ToList();
            //Assert
            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Message);
            Assert.Empty(options);
        }
        [Fact]
        public void ItShould_Return_NotFound_While_Delete_Option()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            CreateOptionRepositoryDataInitializer.Initialize(context);
            var sut = new CreateOptionRepository(context);
            //Act
            var actual = sut.DeleteOption(Guid.Parse("51d34438-a4c6-4e67-86f2-e56380c738b6"));
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(ResponseStatus.NotFound, actual.Status);
            Assert.NotNull(actual.Message);
        }
    }
}
