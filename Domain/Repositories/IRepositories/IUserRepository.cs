using System.Threading.Tasks;
using Data.Models;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Users> AuthenticateUserAsync(int employeeId, string name);
        Task CreateProfile(Profile profile);
        string GetAdminAndChefUserId();

    }
}
