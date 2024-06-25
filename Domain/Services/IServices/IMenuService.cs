using System.Threading.Tasks;
using Domain.ModelDTO;

namespace Domain.Services.IServices
{
    public interface IMenuService
    {
        Task AddMenuItemAsync(MenuItemDTO menuItem);
        Task UpdateMenuItemAsync(MenuItemDTO menuItem);
        Task DeleteMenuItemAsync(int id);
        Task<IEnumerable<MenuItemDTO>> GetMenuItemsAsync();
        Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItemsAsync();
    }
}
