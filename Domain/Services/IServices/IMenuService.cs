using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    internal interface IMenuService
    {
        Task<List<MenuItems>> GetMenuItemsAsync();
        Task AddMenuItemAsync(MenuItems menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int id);
    }
}
