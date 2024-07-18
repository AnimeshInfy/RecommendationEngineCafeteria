using Data.ModelDTO;
using Data.Models;
using Domain.ModelDTO;
using Domain.Models;
using Domain.Services.IServices;
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
            await _ratingService.CalcAvgRatingAsync();
        }

        public async Task CalculateSentimentScore()
        {
            await _sentimentService.CalcSentimentScoreAsync();
        }

        public async Task CreateProfile(Profile profile)
        {
             await _userService.CreateProfile(profile);
        }

        public async Task<IEnumerable<ViewMenuDTO>> GetRecommendedMenuItems(string noOfRecommendedItems)
        {
            await CalculateAvgRating();
            await CalculateSentimentScore();
            return await _menuService.GetRecommendedMenuItemsAsync(noOfRecommendedItems);
        }

        public Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(string userInfo, DateOnly date)
        {
            return _menuService.GetRolledOutItems(userInfo, date);
        }

        public async Task<string> RollOutItems(string[] rollOutIds)
        {
            return await _menuService.RollOutItems(rollOutIds);
        }
    }
}
