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

        public async Task CalculateAverageRatingAsync()
        {
            var feedbackGroupByItemId = await _context.Feedbacks
                                      .GroupBy(u => u.MenuItemId)
                                      .Select(x => new
                                      {
                                          MenuItemId = x.Key,
                                          AverageRating = x.Average(f => f.Rating)
                                      })
                                      .ToListAsync();

            foreach (var group in feedbackGroupByItemId)
            {
                var menuItem = await _context.MenuItems.FindAsync(group.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.AvgRating = group.AverageRating;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
        
}
