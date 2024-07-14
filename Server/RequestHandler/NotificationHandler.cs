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
            var a = request.Split("_");
            await _notificationService.SendNotification(a[1]);
        }
        public async Task<List<Notification>> ViewNotificationsById(string request)
        {
            var notificationInfo = request.Split("_");
            var notificationsById = await _notificationService.ViewNotificationsByUserIdAsync(Convert.ToInt32(notificationInfo[1]));
            var js = JsonConvert.SerializeObject(notificationsById);    
            return notificationsById;
        }
        public async Task<string> ViewNotifications(string request)
        {
            var notifications =  await _notificationService.ViewAllNotificationsAsync();
            return JsonConvert.SerializeObject(notifications);
        }
    }
}
