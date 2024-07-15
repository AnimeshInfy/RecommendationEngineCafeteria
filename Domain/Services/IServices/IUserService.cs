using Data.Models;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IUserService
    {
        Task<Users> AuthenticateUserAsync(int employeeId, string name);
        Task CreateProfile(Profile profile);
    }
}
