using System.Globalization;
using System.Threading.Tasks;
using Data.ModelDTO;
using Domain.ModelDTO;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface IMenuService
    {
        Task AddMenuItemAsync(MenuItemDTO menuItem);
        Task UpdateMenuItemAsync(MenuItemDTO menuItem);
        Task DeleteMenuItemAsync(int id);
        Task DeleteDiscardedMenuItemAsync(int id);
        Task<IEnumerable<ViewMenuDTO>> GetMenuItemsAsync();
        Task ReviewDiscardList();
        Task<IEnumerable<ViewMenuDTO>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems);
        Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(string userInfo, DateOnly date);
        Task<string> RollOutItems(string[] rollOutIds);
        Task ItemsVoting(VotedItems vote);
        Task<string> ViewMaxVotedItems(DateTime currentDate);
        Task<List<MenuItems>> GetDiscardedMenu();
    }
}
