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
            _ratingService.CalculateAverageRatingAsync();
        }

        public async Task CalculateSentimentScore()
        {
            _sentimentService.CalculateSentimentsScore();
        }
        public async Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItems()
        {
            await CalculateAvgRating();
            await CalculateAvgRating();
            return await _menuService.GetRecommendedMenuItemsAsync();
        }
    }
}
