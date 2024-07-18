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
using Data.ModelDTO;
using Domain.Repositories.IRepositories;

namespace Domain.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly INotificationRepository _notificationRepository;

        public MenuService(IMenuItemRepository menuItemRepository,
            INotificationRepository notificationRepository)
        {
            _menuItemRepository = menuItemRepository;
            _notificationRepository = notificationRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task AddMenuItemAsync(MenuItemDTO menuItemDto)
        {
            var menuItem = MapDtoToModel(menuItemDto);
            await _menuItemRepository.AddMenuItemAsync(menuItem);
            string sendToAllUsers = "Y";
            string message = $"{menuItem.Name} has been added to the menu!";
            string sendNotificationrequest = $"{message}:{sendToAllUsers}";
            await _notificationRepository.SendNotification(sendNotificationrequest);
        }

        [Authorize(Roles = "Admin")]
        public async Task UpdateMenuItemAsync(MenuItemDTO menuItemDto)
        {
            var menuItem = MapDtoToModel(menuItemDto);
            string itemName = menuItem.Name;
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
            string sendToAllUsers = "Y";
            string message = $"{itemName} has been updated!";
            string sendNotificationrequest = $"{message}:{sendToAllUsers}";
            await _notificationRepository.SendNotification(sendNotificationrequest);
        }

        [Authorize(Roles = "Admin")]
        public async Task DeleteMenuItemAsync(int id)
        {
            var foodName = _menuItemRepository.GetFoodItemNameById(id).ToString()[0];
            await _menuItemRepository.DeleteMenuItemAsync(id);
            string sendToAllUsers = "Y";
            string message = $"{foodName} has been deleted!";
            string sendNotificationrequest = $"{message}:{sendToAllUsers}";
            await _notificationRepository.SendNotification(sendNotificationrequest);
        }

        public async Task<IEnumerable<ViewMenuDTO>> GetMenuItemsAsync()
        {
            var menuItems = await _menuItemRepository.GetMenuItemsAsync();
            return menuItems.Select(model => MapViewMenuModelToDto(model)).ToList();
        }
        
        public async Task<string> RollOutItems(string[] rolledOutItemsIds)
        {
            return await _menuItemRepository.RollOutItems(rolledOutItemsIds);
        }

        public async Task<IEnumerable<ViewMenuDTO>> GetRecommendedMenuItemsAsync(string noOfRecommendedItems)
        {
            var menuItems = await _menuItemRepository.GetRecommendedMenuItemsAsync(noOfRecommendedItems);
            return menuItems.Select(model => MapViewMenuModelToDto(model)).ToList();
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
                isItemUnderDiscardList = dto.isItemUnderDiscardList,
                dietType = dto.dietType,
                SpiceLevel = dto.SpiceLevel,
                regionalMealPreference = dto.regionalMealPreference,
                isSweetTooth = dto.isItemSweet
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
                isItemUnderDiscardList = model.isItemUnderDiscardList,
                dietType = model.dietType,
                SpiceLevel = model.SpiceLevel,
                regionalMealPreference = model.regionalMealPreference,
                isItemSweet = model.isSweetTooth

            };
        }

        private ViewMenuDTO MapViewMenuModelToDto(MenuItems model)
        {
            return new ViewMenuDTO 
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                MealType = model.MealType,
                Id = model.Id,  
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
                MealType = dto.MealType,
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
                MealType = model.MealType,
            };
        }

        public async Task ItemsVoting(VotedItems vote)
        {
            await _menuItemRepository.ItemsVoting(vote);   
        }

        public async Task DeleteDiscardedMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteDiscardedMenuItemAsync(id);
        }

        public async Task ReviewDiscardList()
        {
            await _menuItemRepository.ReviewMenuItems();
        }

        public async Task<IEnumerable<RolledOutItemsDTO>> GetRolledOutItems(string userInfo, DateOnly date)
        {
            var menuItems = await _menuItemRepository.GetRolledOutItems(userInfo, date);
            return menuItems.Select(model => MapModelToDto(model)).ToList();
        }

        public async Task<string> ViewMaxVotedItems(DateTime currentDate)
        {
            return await _menuItemRepository.ViewMaxVotedItems(currentDate);
        }
    }
}
