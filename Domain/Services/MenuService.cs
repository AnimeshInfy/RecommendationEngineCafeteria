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

        private MenuItems MapDtoToModel(MenuItemDTO dto)
        {
            return new MenuItems
            {
                Id = dto.Id,    
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                MealType = (MealType)Enum.Parse(typeof(MealType), dto.MealType, true)
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
                MealType = model.MealType.ToString()
            };
        }
    }
}
