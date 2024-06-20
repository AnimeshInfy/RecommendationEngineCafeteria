using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class MenuService : IMenuService
    {
        private readonly MenuItemRepository _menuItemRepository;

        public MenuService(MenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<List<MenuItems>> GetMenuItemsAsync()
        {
            return await _menuItemRepository.GetMenuItemsAsync();
        }

        [Authorize(Roles = "Admin")]
        public async Task AddMenuItemAsync(MenuItems menuItem)
        {
            await _menuItemRepository.AddMenuItemAsync(menuItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task UpdateMenuItemAsync(MenuItems menuItem)
        {
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task DeleteMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteMenuItemAsync(id);
        }
    }
}
