using CommunicationSystem.Services.Services.Interfaces;
using Moq;

namespace CommunicationSystem.Tests.Infrastructure.Mocks.GroupRepository
{
    public static class MessageServiceMock
    {
        public static Mock<IMessage> GetMock()
        {
            var mock = new Mock<IMessage>();
            return mock;
        }
    }
}
