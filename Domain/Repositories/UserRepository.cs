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

        public async Task<Users> AuthenticateUserAsync(int employeeId, string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == employeeId && u.Name == name);
        }
    }
}
