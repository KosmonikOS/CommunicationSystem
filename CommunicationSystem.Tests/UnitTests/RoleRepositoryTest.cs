using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using System.Linq;
using Xunit;

namespace CommunicationSystem.Tests.UnitTests
{
    public class RoleRepositoryTest
    {
        [Fact]
        public void ItShould_Create_Instance()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            var sut = new RoleRepository(context);
            //Act
            //Assert
            Assert.NotNull(sut);
        }
        [Fact]
        public void ItShould_Get_Roles()
        {
            //Arrange
            var context = DbContextHelper.CreateInMemoryContext();
            RoleRepositoryDataInitializer.Initialize(context);
            var sut = new RoleRepository(context);
            //Act
            var actual = sut.GetRoles().ToList();
            //Assert
            Assert.Equal(3, actual.Count);
        }
    }
}
