using CommunicationSystem.Models;
using CommunicationSystem.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Services
{
    public class ConfirmationTokenService : IConfirmationToken
    {
        private readonly CommunicationContext db;

        public ConfirmationTokenService(CommunicationContext db)
        {
            this.db = db;
        }
        public async Task ConfirmTokenAsync(string token)
        {
            var user = db.Users.SingleOrDefault(u => u.IsConfirmed == token);
            if (user != null)
            {
                var timeStamp = Convert.ToDateTime(Encoding.UTF8.GetString(Convert.FromBase64String(token.Split("@d@")[1]))).AddSeconds(3600);
                if (timeStamp >= DateTime.Now)
                {
                    user.IsConfirmed = "true";
                    db.Users.Update(user);
                }
                else
                {
                    db.Users.Remove(user);
                }
                await db.SaveChangesAsync();
            }
        }

        public string GenerateToken(string email)
        {
            var token = Convert.ToBase64String(Encoding.ASCII.GetBytes(email)) + "@d@" + Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
            return token;
        }
    }
}
