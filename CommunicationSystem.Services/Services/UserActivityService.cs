using CommunicationSystem.Services.Services.Interfaces;

namespace CommunicationSystem.Services
{
    public class UserActivityService : IUserActivity
    {
        public string GetUserActivity(DateTime? enter, DateTime? leave)
        {
            if (enter != null && leave != null)
            {
                var type = leave?.Date == DateTime.Today ? "t" : leave?.Year == DateTime.Now.Year ? "M" : "d";
                var seporator = leave?.Date == DateTime.Today ? " в" : ":";
                return enter > leave ? "В сети" : $"Был(a) в сети{seporator} {leave?.ToString(type)}";
            }
            return "Не в сети";
        }
    }
}
