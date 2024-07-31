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
        Task DeleteDiscardedMenuItemAsync(int id);
        Task<IEnumerable<MenuItems>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems);
        Task<IEnumerable<RolledOutItems>> GetRolledOutItems(string userInfo, DateOnly date);
        Task<string> RollOutItems(string[] rollOutIds);
        Task ItemsVoting(VotedItems vote);
        Task ReviewMenuItems();
        Task<List<string>> GetFoodItemNameById(int foodId);
        bool isFoodItemUnderDiscardMenu(int foodId);
        Task<string> ViewMaxVotedItems(DateTime currentDate);
        Task<List<MenuItems>> GetDiscardedMenu();
    }
}
