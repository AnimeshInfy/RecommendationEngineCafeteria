using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItems>> GetMenuItemsAsync();
        Task AddMenuItemAsync(MenuItems menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int id);
    }
}
