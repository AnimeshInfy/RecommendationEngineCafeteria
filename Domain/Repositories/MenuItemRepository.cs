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

        public async Task<IEnumerable<MenuItems>> GetRecommendedMenuBasedOnProfileAsync()
        {
            return await _context.MenuItems
                .OrderByDescending(x => x.isSweetTooth)
                .ThenBy(x => x.dietType)
                .ThenBy(x => x.SpiceLevel)
                .ThenByDescending(x => x.CommonScore)
                .ToListAsync();
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

        public async Task<IEnumerable<RolledOutItems>> GetRolledOutItems(DateOnly date)
        {
            date = DateOnly.FromDateTime(DateTime.Now);
            return await _context.RolledOutItems.Where(x => x.RolledOutDate.Date ==
            date.ToDateTime(TimeOnly.MinValue).Date).ToListAsync();
        }

        public async Task ItemsVoting(Dictionary<string, string> mealVotes)
        {
            foreach (var vote in mealVotes)
            {
                string mealType = vote.Key;
                string mealName = vote.Value;

                await CastVoteAsync(mealType, mealName);
            }
        }

        public async Task CastVoteAsync(string mealType, string foodName)
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine("\nEnter your vote: ");
            Console.WriteLine("Enter your user id: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            var vote = new VotedItems
            {
                Date = date.ToDateTime(TimeOnly.MinValue).Date,
                MealTypes = mealType,
                FoodName = foodName,
                UserId = userId
            };

            _context.VotedItems.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscardedMenuItemAsync(int id)
        {
            var discardedMenuItem = await _context.MenuItems.FirstOrDefaultAsync(x => x.isItemUnderDiscardList == true
            && x.Id == id);
            if (discardedMenuItem != null)
            {
                _context.MenuItems.Remove(discardedMenuItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                await Console.Out.WriteLineAsync("\nRecord not found");
            }
        }

        public async Task ReviewMenuItems()
        {
            var menuItems = await _context.MenuItems.ToListAsync();
            foreach (var item in menuItems)
            {
                var ratingScore = item.AvgRating * 7;
                var sentimentScore = item.SentimentScore * 3;
                var discardScore = ratingScore + sentimentScore;
                if (discardScore < 20)
                {
                    item.isItemUnderDiscardList = true;
                }
                _context.MenuItems.Update(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<string>> GetFoodItemNameById(int foodId)
        {
            var menuItemFoodName = await _context.MenuItems.Where(x => x.Id == foodId).
                Select(x => x.Name).ToListAsync();
            return menuItemFoodName;
        }
        public bool isFoodItemUnderDiscardMenu(int foodId)
        {
            var menuItems = _context.MenuItems.Where(x => x.Id == foodId && x.isItemUnderDiscardList == true).ToList();  
            if (menuItems.Count > 0)
            {
                return true;
            }
            return false;
        }
    }

}
