using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.Helpers
{
    public static class FixtureHelper
    {
        public static Fixture Fixture
        {
            get
            {
                var fixture = new Fixture();
                fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                return fixture;
            }
        }

    }
}
