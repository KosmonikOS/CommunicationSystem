using CommunicationSystem.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
