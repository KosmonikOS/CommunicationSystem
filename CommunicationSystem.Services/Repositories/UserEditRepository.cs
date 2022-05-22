using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Repositories
{
    public class UserEditRepository : IUserEditRepository
    {
        private readonly CommunicationContext db;

        public UserEditRepository(CommunicationContext db)
        {
            this.db = db;
        }
        public async Task DeleteUserAsync(int id)
        {
            if(id != 0)
            {
                var user = db.Users.SingleOrDefault(u => u.Id == id);
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            var roles = await db.Roles.AsNoTracking().ToListAsync();
            return roles;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await (from u in db.Users
                               join r in db.Roles on u.Role equals r.Id
                               orderby u.Role descending
                               select new User()
                               {
                                   Id = u.Id,
                                   Role = u.Role,
                                   RoleName = r.Name,
                                   FirstName = u.FirstName,
                                   accountImage = u.accountImage,
                                   Email = u.Email,
                                   Grade = u.Grade,
                                   GradeLetter = u.GradeLetter,
                                   IsConfirmed = u.IsConfirmed,
                                   LastName = u.LastName,
                                   MiddleName = u.MiddleName,
                                   NickName = u.NickName,
                                   Password = u.Password,
                                   Phone = u.Phone
                               }
                ).AsNoTracking().ToListAsync();
            return users;
        }

        public async Task SaveUserAsync(User user)
        {
            if (user != null)
            {
                if (user.Id != 0)
                {
                    db.Users.Update(user);
                }
                else
                {
                    user.IsConfirmed = "true";
                    db.Users.Add(user);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
