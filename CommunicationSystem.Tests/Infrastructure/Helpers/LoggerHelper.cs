using Microsoft.Extensions.Logging;
using Moq;

namespace CommunicationSystem.Tests.Infrastructure.Helpers
{
    public static class LoggerHelper
    {
        public static ILogger<T> GetLogger<T>()
        {
            var mock = new Mock<ILogger<T>>();
            return mock.Object;
        }
    }
}
