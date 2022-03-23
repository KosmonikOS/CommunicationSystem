using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CommunicationContext db;

        public AuthRepository(CommunicationContext db)
        {
            this.db = db;
        }

        public User GetConfirmedUser(Login user)
        {
            return db.Users.SingleOrDefault(u => u.Email == user.Email && u.Password == user.Password && u.IsConfirmed == "true");
        }

        public async Task SetTimeAsync(int id, string act)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            switch (act)
            {
                case "enter":
                    user.EnterTime = DateTime.Now;
                    break;
                case "leave":
                    user.LeaveTime = DateTime.Now;
                    break;
            }
            db.Users.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
