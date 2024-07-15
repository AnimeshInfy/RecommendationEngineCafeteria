using Core.Utilities;
using Data.Models;
using Domain.ModelDTO;
using Domain.Models;
using Domain.Services.IServices;
using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RecommendationEngineService : IRecommendationEngineService
    {
        private readonly IRatingServce _ratingService;
        private readonly ISentimentsAnalysisService _sentimentService;
        private readonly IMenuService _menuService; 
        private readonly IUserService _userService;
        public RecommendationEngineService(IRatingServce ratingServce, 
            ISentimentsAnalysisService sentimentService, IMenuService menuService,
            IUserService userService)
        {
            _ratingService = ratingServce;
            _sentimentService = sentimentService;
            _menuService = menuService;
            _userService = userService;
        }

        public async Task CalculateAvgRating()
        {
            await _ratingService.CalculateAverageRatingAsync();
        }

        public async Task CalculateSentimentScore()
        {
            await _sentimentService.CalculateSentimentsScore();
        }

        public async Task CreateProfile(Profile profile)
        {
             await _userService.CreateProfile(profile);
        }

        public async Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItems(string noOfRecommendedItems)
        {
            await CalculateAvgRating();
            await CalculateSentimentScore();
            return await _menuService.GetRecommendedMenuItemsAsync(noOfRecommendedItems);
        }

        public Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(DateOnly date)
        {
            return _menuService.GetRolledOutItems(date);
        }

        public async Task RollOutItems(string[] rollOutIds)
        {
            await _menuService.RollOutItems(rollOutIds);
        }
    }
}
