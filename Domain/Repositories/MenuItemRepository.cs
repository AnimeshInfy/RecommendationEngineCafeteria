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

        public async Task<string> RollOutItems(string[] rollOutIds)
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
                    isItemUnderDiscardList = menuItem.isItemUnderDiscardList,
                    dietType = menuItem.dietType,
                    SpiceLevel = menuItem.SpiceLevel,
                    regionalMealPreference = menuItem.regionalMealPreference,
                    isSweetTooth = menuItem.isSweetTooth,
                };

                await _context.RolledOutItems.AddAsync(rolledOutItem);
                rolledOutItems.Add(rolledOutItem);
            }

            await _context.SaveChangesAsync();
            return "Items rolled out successfully";
        }

        public async Task<IEnumerable<RolledOutItems>> GetRolledOutItems(string userInfo, DateOnly date)
        {
            try
            {
                date = DateOnly.FromDateTime(DateTime.Now);
                var profile = await _context.Profile.Where(x => x.UserId == Convert.ToInt32(userInfo)).FirstOrDefaultAsync();
                var rolledOutItemsByChef = await _context.RolledOutItems.ToListAsync();
                var menuRecommended = rolledOutItemsByChef
                    .Where(x => DateOnly.FromDateTime(x.RolledOutDate) == date)
                    .ToList();
                List<RolledOutItems> menuSortedOnProfile = new List<RolledOutItems>();

                if (profile != null && menuRecommended != null)
                {
                    var sortedMenu = profile.isSweetTooth.ToLower() == "yes"
                        ? menuRecommended.OrderByDescending(item => item.isSweetTooth)
                        : menuRecommended.OrderBy(item => item.isSweetTooth);

                    var nullCheckOnDietType = sortedMenu.Where(x => x.dietType != null);
                    if (nullCheckOnDietType != null)
                    {
                        sortedMenu = profile.dietType.ToLower() == "veg"
                        ? sortedMenu.ThenByDescending(item => item.dietType)
                        : sortedMenu.ThenBy(item => item.dietType);
                    }

                    var nullCheckOnRegionalMealChoice = sortedMenu.Where(x => x.regionalMealPreference != null);
                    if (nullCheckOnRegionalMealChoice != null)
                    {
                        if (profile.regionalMealPreference.ToLower() == "north")
                        {
                            sortedMenu = sortedMenu.ThenBy(item => item.regionalMealPreference);
                        }
                        else if (profile.regionalMealPreference.ToLower() == "south")
                        {
                            sortedMenu = sortedMenu.ThenByDescending(item => item.regionalMealPreference == "South");
                        }
                    }
                    
                    var nullCheckOnSpiceLevel = sortedMenu.Where(x => x.SpiceLevel != null);
                    if (nullCheckOnSpiceLevel != null)
                    {
                        switch (profile.SpiceLevel.ToLower())
                        {
                            case "high":
                                sortedMenu = sortedMenu.ThenBy(item => item.SpiceLevel == "High" ? 0 : 1);
                                break;
                            case "medium":
                                sortedMenu = sortedMenu.ThenBy(item => item.SpiceLevel == "Medium" ? 0 : 1);
                                break;
                            default:
                                sortedMenu = sortedMenu.ThenBy(item => item.SpiceLevel == "Low" ? 0 : 1);
                                break;
                        }
                    }
                    menuSortedOnProfile = sortedMenu.ToList();
                }
                return menuSortedOnProfile;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<RolledOutItems>();
            }
        }

        public async Task ItemsVoting(VotedItems vote)
        {
            await _context.VotedItems.AddAsync(vote);
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

        public async Task<string> ViewMaxVotedItems(DateTime currentDate)
        {
            var voteItem = await _context.VotedItems
           .Where(item => item.Date.Date == currentDate.Date)
           .GroupBy(item => item.FoodName)
           .Select(group => new
           {
               FoodName = group.Key,
               Count = group.Count()
           })
           .OrderByDescending(group => group.Count)
           .FirstOrDefaultAsync();

            return $"Maximum voted item: {voteItem.FoodName}";
        }
    }
}
