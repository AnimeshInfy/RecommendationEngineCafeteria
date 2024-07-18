using Data.ModelDTO;
using Data.Models;
using Domain.ModelDTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IRecommendationEngineService
    {
        Task CalculateSentimentScore();
        Task CalculateAvgRating();
        Task<IEnumerable<ViewMenuDTO>> GetRecommendedMenuItems(string noOfRecommendedItems);
        Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(string userInfo, DateOnly date);
        Task<string> RollOutItems(string[] rollOutIds);
        Task CreateProfile(Profile profile);
    }
}
