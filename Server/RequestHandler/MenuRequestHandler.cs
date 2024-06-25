using Domain.Services.IServices;
using Domain.ModelDTO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Server.RequestHandler
{
    public class MenuRequestHandler
    {
        private readonly IMenuService _menuService;
        private readonly IRecommendationEngineService _recommendationEngineService;

        public MenuRequestHandler(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<string> HandleRequestAsync(string request)
        {
            if (request.Contains("AddItem"))
            {
                var a = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<MenuItemDTO>(a[1]);
                await _menuService.AddMenuItemAsync(jsonDeserialized);
                return "Item added successfully";
            }
            else if (request.Contains("UpdateItem"))
            {
                var a = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<MenuItemDTO>(a[1]);
                await _menuService.UpdateMenuItemAsync(jsonDeserialized);
                return "Item updated successfully";
            }
            else if (request.Contains("DeleteItem"))
            {
                var a = request.Split('_');
                int itemId = int.Parse(a[1]);
                await _menuService.DeleteMenuItemAsync(itemId);
                return "Item deleted successfully";
            }
            else if (request.Contains("ViewMenu"))
            {
                var menuItems = await _menuService.GetMenuItemsAsync();
                return JsonConvert.SerializeObject(menuItems);
            }

            return "Unknown Request";
        }
    }
}
