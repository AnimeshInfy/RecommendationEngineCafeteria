using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CafeteriaDbContext _context;

        public UserRepository(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task<Users> AuthenticateUserAsync(int id, string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Name == name);
        }

        public string GetUserRole(int id, string name)
        {
            string role = "";
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Name == name);
            if (user == null) 
            {
                role = user.Role;
            }
            role = "INVALID CREDENTIALS";
            return role;
        }

        public string GetAdminAndChefUserId()
        {
            var adminAndChefUsersId = _context.Users.Where(x => x.Role.ToLower() == "admin"
            || x.Role.ToLower() == "chef").Select(x => x.Id).ToList();
            string targetUserIds = string.Join(",", adminAndChefUsersId);
            return targetUserIds;
        }
    }
}
