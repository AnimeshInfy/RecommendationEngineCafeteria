using Domain.DataAccess;
using Domain.ModelDTO;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly CafeteriaDbContext _context;

        public MenuItemRepository(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItems>> GetMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task AddMenuItemAsync(MenuItems menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(MenuItems menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<MenuItems>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems)
        {
            var noOfRecommendedItemsInt = Convert.ToInt32(noOfRecommendedItems);
            var menuItems = await _context.MenuItems.OrderByDescending(x => x.CommonScore).Take(noOfRecommendedItemsInt).ToListAsync();
            return menuItems;
        }

        public async Task RollOutItems(string[] rollOutIds)
        {
            var rollOutIdsInt = rollOutIds.Select(id => int.Parse(id)).ToArray();

            var rolledOutMenuItems = await _context.MenuItems
                                                   .Where(x => rollOutIdsInt.Contains(x.Id))
                                                   .ToListAsync();

            var rolledOutItems = new List<RolledOutItems>();

            foreach (var menuItem in rolledOutMenuItems)
            {
                var rolledOutItem = new RolledOutItems
                {
                    Name = menuItem.Name,
                    RolledOutDate = DateTime.Now,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    IsAvailable = menuItem.IsAvailable,
                    MealType = menuItem.MealType,
                    AvgRating = menuItem.AvgRating,
                    SentimentScore = menuItem.SentimentScore,
                    CommonScore = menuItem.CommonScore,
                    isItemUnderDiscardList = menuItem.isItemUnderDiscardList
                };

                await _context.RolledOutItems.AddAsync(rolledOutItem);
                rolledOutItems.Add(rolledOutItem);
            }

            await _context.SaveChangesAsync();
        }
    }

}
