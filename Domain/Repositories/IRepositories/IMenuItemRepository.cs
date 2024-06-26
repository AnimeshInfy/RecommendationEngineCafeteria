using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ModelDTO;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItems>> GetMenuItemsAsync();
        Task AddMenuItemAsync(MenuItems menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int id);
        Task<IEnumerable<MenuItems>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems);
        Task RollOutItems(string[] rollOutIds);
    }
}
