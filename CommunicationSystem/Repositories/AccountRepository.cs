﻿using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CommunicationContext db;

        public AccountRepository(CommunicationContext db)
        {
            this.db = db;
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.SingleOrDefault(u => u.Email == email);
        }

        public async Task UpdateImageAsync(int id, string path)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            user.accountImage = path;
            db.Users.Update(user);
            await db.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
