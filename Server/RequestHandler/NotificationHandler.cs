using Domain.Models;
using Domain.Services;
using Domain.Services.IServices;
using Newtonsoft.Json;
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
            var notificationInfo = request.Split("_");
            await _notificationService.SendNotification(notificationInfo[1]);
        }
        public async Task<string> ViewNotificationsById(string request)
        {
            var notificationInfo = request.Split("_");
            var notificationsById = await _notificationService.ViewNotificationsByUserIdAsync(Convert.ToInt32(notificationInfo[1]));
            string result = "";

            foreach (var notification in notificationsById)
            {
                result += ($"{notification.NotificationDate.ToString("yyyy-MM-dd HH:mm:ss")}: {notification.Message}\n");
            }

            return result;

        }
        public async Task<string> ViewNotifications(string request)
        {
            var notifications =  await _notificationService.ViewAllNotificationsAsync();
            string result = "";

            foreach (var notification in notifications)
            {
                result += ($"{notification.NotificationDate.ToString("yyyy-MM-dd HH:mm:ss")}: {notification.Message}\n");
            }

            return result;
        }
    }
}
