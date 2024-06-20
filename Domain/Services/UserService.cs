using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users> AuthenticateUserAsync(int employeeId, string name)
        {
            return await _userRepository.AuthenticateUserAsync(employeeId, name);
        }

        public async Task<string> GetRoleForUserAsync(int employeeId, string name)
        {
            var user = await _userRepository.AuthenticateUserAsync(employeeId, name);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User Not found or Invalid credentials");
            }

            return user.Role; 
        }
    }
}
