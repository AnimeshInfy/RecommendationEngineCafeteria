using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Users> AuthenticateUserAsync(int employeeId, string name);
    }
}
