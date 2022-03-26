using System;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IUserActivity
    {
        public string GetUserActivity(DateTime? enter, DateTime? leave);
    }
}
