using Core.Utilities;
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
        public RecommendationEngineService(IRatingServce ratingServce, 
            ISentimentsAnalysisService sentimentService, IMenuService menuService)
        {
            _ratingService = ratingServce;
            _sentimentService = sentimentService;
            _menuService = menuService;
        }

        public async Task CalculateAvgRating()
        {
            await _ratingService.CalculateAverageRatingAsync();
        }

        public async Task CalculateSentimentScore()
        {
            await _sentimentService.CalculateSentimentsScore();
        }
        public async Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItems(string noOfRecommendedItems)
        {
            await CalculateAvgRating();
            await CalculateSentimentScore();
            return await _menuService.GetRecommendedMenuItemsAsync(noOfRecommendedItems);
        }

        public Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems()
        {
            return _menuService.GetRolledOutItems();
        }

        public async Task RollOutItems(string[] rollOutIds)
        {
            await _menuService.RollOutItems(rollOutIds);
        }
    }
}
