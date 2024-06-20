using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CafeteriaDbContext _context;
        public NotificationRepository(CafeteriaDbContext context)
        {
            _context = context;
        }
        public void SendNotification()
        {
            //Not implemented yet
        }

        public async Task<List<Notification>> ViewAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }
    }
}
