using Domain.Services.IServices;
using Domain.ModelDTO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Domain.Models;

namespace Server.RequestHandler
{
    public class MenuRequestHandler
    {
        private readonly IMenuService _menuService;
        private readonly IRecommendationEngineService _recommendationEngineService;
        private readonly IRatingServce _ratingServce;
        private readonly ISentimentsAnalysisService _sentimentsAnalysisService;
        public MenuRequestHandler(IMenuService menuService, IRatingServce ratingServce, 
            ISentimentsAnalysisService sentimentsAnalysisService)
        {
            _menuService = menuService;
            _ratingServce = ratingServce;
            _sentimentsAnalysisService = sentimentsAnalysisService;
        }

        public async Task<string> HandleRequestAsync(string request)
        {
            if (request.Contains("AddItem"))
            {
                var menuItemsInfo = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<MenuItemDTO>(menuItemsInfo[1]);
                await _menuService.AddMenuItemAsync(jsonDeserialized);
                return "Item added successfully";
            }
            else if (request.Contains("UpdateItem"))
            {
                var menuItemsInfo = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<MenuItemDTO>(menuItemsInfo[1]);
                await _menuService.UpdateMenuItemAsync(jsonDeserialized);
                return "Item updated successfully";
            }
            else if (request == "DeleteItem")
            {
                var menuItemsInfo = request.Split('_');
                int itemId = int.Parse(menuItemsInfo[1]);
                await _menuService.DeleteMenuItemAsync(itemId);
                return "Item deleted successfully";
            }
            else if (request.Contains("DeleteItemsFromDiscardList"))
            {
                var menuItemsInfo = request.Split('_');
                int itemId = int.Parse(menuItemsInfo[1]);
                await _menuService.DeleteDiscardedMenuItemAsync(itemId);
                return "Item deleted successfully";
            }
            else if (request.Contains("ViewMenu"))
            {
                var menuItems = await _menuService.GetMenuItemsAsync();
                return JsonConvert.SerializeObject(menuItems);
            }
            else if (request.Contains("ReviewMenuItems"))
            {
                await _menuService.ReviewDiscardList();
                return "Menu items have been reviewed for discard list";
            }

            return "Unknown Request";
        }

        public async Task<string> ViewMaxVotedItems(string request)
        {
            var voteItemsInfo = request.Split("_");
            var currentDate = Convert.ToDateTime(voteItemsInfo[1]).Date;
            return await _menuService.ViewMaxVotedItems(currentDate);
        }

        public async Task<string> CalcAvgRatingAsync(string request)
        {
            return await _ratingServce.CalcAvgRatingAsync();
        }

        public async Task<string> CalcSentimentScoreAsync(string request)
        {
            return await _sentimentsAnalysisService.CalcSentimentScoreAsync();
        }
        public async Task<string> GetDiscardedMenu(string request)
        {
            var discardedMenu = await _menuService.GetDiscardedMenu();
            return JsonConvert.SerializeObject(discardedMenu);
        }
    }
}
