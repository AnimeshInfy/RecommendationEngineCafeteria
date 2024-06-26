using Domain.Models;
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
                var a = request.Split('_');
                var noOfRecommendedItems = a[1];
                var menuItems = await _recommendationEngineService.GetRecommendedMenuItems(noOfRecommendedItems);
                return JsonConvert.SerializeObject(menuItems);
            }
            return "Unknown Request";
        }
        public async Task RollOutItems(string request)
        {
            if (request.Contains("RollOutItems"))
            {
                var a = request.Split("_");
                var rollOutIds = a[1].Split(",");
                await _recommendationEngineService.RollOutItems(rollOutIds);
            }
        }
    }
}


