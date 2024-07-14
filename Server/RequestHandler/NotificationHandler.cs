using Domain.Models;
using Domain.Services;
using Domain.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.RequestHandler
{
    public class NotificationHandler
    {
        private INotificationService _notificationService;
        public NotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;

        }
        public async Task SendNotifications(string request)
        {
            var a = request.Split("_");
            await _notificationService.SendNotification(a[1]);
        }
        public async Task ViewNotificationsById(string request)
        {
            await _notificationService.ViewNotificationsByUserIdAsync();
        }
        public async Task ViewNotifications(string request)
        {
            await _notificationService.ViewAllNotificationsAsync();
        }
    }
}
