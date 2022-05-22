using CommunicationSystem.Data;
using CommunicationSystem.Services.Services.Interfaces;

namespace CommunicationSystem.Services
{
    public class RegistrationService : IRegistration
    {
        private readonly CommunicationContext db;

        public RegistrationService(CommunicationContext db)
        {
            this.db = db;
        }
        public bool IsUniqueEmail(string email)
        {
            var result = db.Users.SingleOrDefault(u => u.Email == email);
            return result == null;
        }
    }
}
