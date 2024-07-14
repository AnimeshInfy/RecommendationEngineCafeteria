using System.Globalization;
using System.Threading.Tasks;
using Domain.ModelDTO;

namespace Domain.Services.IServices
{
    public interface IMenuService
    {
        Task AddMenuItemAsync(MenuItemDTO menuItem);
        Task UpdateMenuItemAsync(MenuItemDTO menuItem);
        Task DeleteMenuItemAsync(int id);
        Task DeleteDiscardedMenuItemAsync(int id);
        Task<IEnumerable<MenuItemDTO>> GetMenuItemsAsync();
        Task ReviewDiscardList();
        Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems);
        Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(DateOnly date);
        Task RollOutItems(string[] rollOutIds);
        Task ItemsVoting(Dictionary<string, string> mealVotes);
        Task CastVoteAsync(string mealType, string foodName);
    }
}
