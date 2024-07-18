using Data.Models;
using Domain.ModelDTO;
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
        private readonly IMenuService _menuService;
        public RecommendationHandler(IRecommendationEngineService recommendationEngineService, 
            IMenuService menuService)
        {
            _recommendationEngineService = recommendationEngineService;
            _menuService = menuService;
        }
        public async Task<string> GetRecommendedMeals(string request)
        {
            if (request.Contains("GetRecommendedMeals"))
            {
                var recommendedMealsInfo = request.Split('_');
                var noOfRecommendedItems = recommendedMealsInfo[1];
                var menuItems = await _recommendationEngineService.GetRecommendedMenuItems(noOfRecommendedItems);
                return JsonConvert.SerializeObject(menuItems);
            }
            return "Unknown Request";
        }
        public async Task<string> RollOutItems(string request)
        {
            if (request.Contains("RollOutItems"))
            {
                var rollOutItemsInfo = request.Split("_");
                var rollOutIds = rollOutItemsInfo[1].Split(",");
                var rollOutItemsString = await _recommendationEngineService.RollOutItems(rollOutIds);
                if (rollOutItemsString!= null)
                {
                    return "Items Rolled out Successfully!";
                }
                return "Roll out Failed";
            }
            return "Invalid Request";
        }

        public async Task<string> GetRolledOutItems(string request)
        {
            if (request.Contains("GetRolledOutItems"))
            {
                var userInfo = request.Split("_")[1];   
                DateOnly dateOnly = new DateOnly();
                var menuItems = await _recommendationEngineService.GetRolledOutItems(userInfo, dateOnly);
                return JsonConvert.SerializeObject(menuItems);
            }
            return "Unknown request";
        }

        public async Task<string> ItemsVoting(string request)
        {
            if (request.Contains("ItemsVoting"))
            {
                var itemVotingInfo = request.Split("_");
                if (itemVotingInfo.Length > 1)
                {
                    var itemVotingInfoDeserialized = JsonConvert.DeserializeObject<VotedItems>(itemVotingInfo[1]);
                    await _menuService.ItemsVoting(itemVotingInfoDeserialized);
                    return "Voted Successfully";
                }
                return "Voting Unsuccessful";
            }
            return "Unknown request";
        }

        public async Task<string> CreateProfile(string request)
        {
            var profileInfo = request.Split('_');
            var jsonDeserialized = JsonConvert.DeserializeObject<Profile>(profileInfo[1]);
            await _recommendationEngineService.CreateProfile(jsonDeserialized);
            return "Profile created successfully";
        }
    }
}


