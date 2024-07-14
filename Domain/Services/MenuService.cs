using Domain.ModelDTO;
using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Domain.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task AddMenuItemAsync(MenuItemDTO menuItemDto)
        {
            var menuItem = MapDtoToModel(menuItemDto);
            await _menuItemRepository.AddMenuItemAsync(menuItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task UpdateMenuItemAsync(MenuItemDTO menuItemDto)
        {
            var menuItem = MapDtoToModel(menuItemDto);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task DeleteMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteMenuItemAsync(id);
        }

        public async Task<IEnumerable<MenuItemDTO>> GetMenuItemsAsync()
        {
            var menuItems = await _menuItemRepository.GetMenuItemsAsync();
            return menuItems.Select(model => MapModelToDto(model)).ToList();
        }
        
        public async Task RollOutItems(string[] rolledOutItemsIds)
        {
            await _menuItemRepository.RollOutItems(rolledOutItemsIds);
        }

        public async Task<IEnumerable<MenuItemDTO>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems)
        {
            var menuItems = await _menuItemRepository.GetRecommendedMenuItemsAsync(noOfRecommendedItems);
            return menuItems.Select(model => MapModelToDto(model)).ToList();
        }

        private MenuItems MapDtoToModel(MenuItemDTO dto)
        {
            return new MenuItems
            {
                Id = dto.Id,    
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                MealType = (MealType)Enum.Parse(typeof(MealType), dto.MealType, true),
                AvgRating = dto.AvgRating,
                SentimentScore = dto.SentimentScore,
                CommonScore = dto.CommonScore,
                isItemUnderDiscardList = dto.isItemUnderDiscardList
            };
        }

        private MenuItemDTO MapModelToDto(MenuItems model)
        {
            return new MenuItemDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                IsAvailable = model.IsAvailable,
                MealType = model.MealType.ToString(),
                AvgRating = model.AvgRating,
                SentimentScore = model.SentimentScore,
                CommonScore = model.CommonScore,
                isItemUnderDiscardList = model.isItemUnderDiscardList
            };
        }

        private RolledOutItems MapDtoToModel(RolledOutItemsDTO dto)
        {
            return new RolledOutItems
            {
                Id = dto.Id,
                Name = dto.Name,
                RolledOutDate = DateTime.Now,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                MealType = dto.MealType,
                AvgRating = dto.AvgRating,
                SentimentScore = dto.SentimentScore,
                CommonScore = dto.CommonScore,
                isItemUnderDiscardList = dto.isItemUnderDiscardList
            };
        }

        private RolledOutItemsDTO MapModelToDto(RolledOutItems model)
        {
            return new RolledOutItemsDTO
            {
                Id = model.Id,
                Name = model.Name,
                RolledOutDate = DateTime.Now,
                Description = model.Description,
                Price = model.Price,
                IsAvailable = model.IsAvailable,
                MealType = model.MealType,
                AvgRating = model.AvgRating,
                SentimentScore = model.SentimentScore,
                CommonScore = model.CommonScore,
                isItemUnderDiscardList = model.isItemUnderDiscardList
            };
        }
        public async Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(DateOnly date)
        {
            var menuItems = await _menuItemRepository.GetRolledOutItems(date);
            return menuItems.Select(model => MapModelToDto(model)).ToList();
        }

        public async Task ItemsVoting(Dictionary<string, string> mealVotes)
        {
            await _menuItemRepository.ItemsVoting(mealVotes);   
        }

        public async Task CastVoteAsync(string mealType, string foodName)
        {
            await _menuItemRepository.CastVoteAsync(mealType, foodName);    
        }

        public async Task DeleteDiscardedMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteDiscardedMenuItemAsync(id);
        }
    }
}
