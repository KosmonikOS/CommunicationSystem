using CommunicationSystem.Services.Services.Interfaces;
using Moq;

namespace CommunicationSystem.Tests.Infrastructure.Mocks
{
    public static class PasswordHashServiceMock
    {
        public static Mock<IPasswordHashService> GetMock()
        {
            var mock = new Mock<IPasswordHashService>();
            mock.Setup(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), "true"))
                .Returns(true);
            mock.Setup(x => x.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), "false"))
                .Returns(false);

            return mock;
        }
    }
}
