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
        Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItems(string noOfRecommendedItems);
        Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems();
        Task RollOutItems(string[] rollOutIds);
    }
}
