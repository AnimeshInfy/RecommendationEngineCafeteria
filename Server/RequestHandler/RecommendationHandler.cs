using Domain.Services;
using Domain.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.RequestHandler
{
    public class RecommendationHandler
    {
        private readonly IRecommendationEngineService _recommendationEngineService;
        public RecommendationHandler(IRecommendationEngineService recommendationEngineService)
        {
            _recommendationEngineService = recommendationEngineService;
        }
        public async Task<string> GetRecommendedMeals(string request)
        {
            if (request.Contains("GetRecommendedMeals"))
            {
                //var a = request.Split('_');
                //var recommemndedMenu = await _recommendationEngineService.GetRecommendedFoodItems();
                //return JsonConvert.SerializeObject(recommemndedMenu);
                var menuItems = await _recommendationEngineService.GetRecommendedMenuItems();
                return JsonConvert.SerializeObject(menuItems);
            }
            return "Unknown Request";
        }
    }
}
