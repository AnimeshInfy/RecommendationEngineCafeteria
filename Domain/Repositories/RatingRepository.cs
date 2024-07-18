using Domain.DataAccess;
using Domain.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private CafeteriaDbContext _context;
        public RatingRepository(CafeteriaDbContext context) 
        {
            _context = context;
        }

        public async Task<string> CalcAvgRatingAsync()
        {
            var feedbackGroupByItemId = await _context.Feedbacks
                                                .GroupBy(f => f.MenuItemId)
                                                .Select(g => new
                                                {
                                                    MenuItemId = g.Key,
                                                    AverageRating = g.Average(f => f.Rating)
                                                })
                                                .ToListAsync();

            var menuItemIds = feedbackGroupByItemId.Select(f => f.MenuItemId).ToList();
            var menuItems = await _context.MenuItems
                                         .Where(mi => menuItemIds.Contains(mi.Id))
                                         .ToListAsync();

            foreach (var menuItem in menuItems)
            {
                var averageRating = feedbackGroupByItemId
                                    .FirstOrDefault(f => f.MenuItemId == menuItem.Id)?.AverageRating;

                if (averageRating.HasValue)
                {
                    menuItem.AvgRating = averageRating.Value;
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
            return "Ratings Calc Successfully";
        }
    }
        
}
