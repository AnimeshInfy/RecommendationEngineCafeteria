using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public async Task SendNotification(string request)
        {
            try
            {
                Notification notification = new Notification();
                var notificationInfo = request.Split(':');
                string message = notificationInfo[0];
                notification.Message = message;
                string isSendToAllUsers = "Y";
                string targetedUserIds = "";
                var receiversInfo = notificationInfo[1].Split('-');
                if (receiversInfo.Length > 1)
                {
                    isSendToAllUsers = receiversInfo[0];
                    targetedUserIds = receiversInfo[1];
                }

                notification.NotificationDate = DateTime.Now;

                if (isSendToAllUsers == "Y")
                {
                    List<int> userId = await _context.Users.Select(x => x.Id).ToListAsync();
                    string usersJson = JsonSerializer.Serialize(userId);
                    List<int> userIds = JsonSerializer.Deserialize<List<int>>(usersJson);
                    notification.TargetedUserIds = string.Join(",", userIds);
                }
                else
                {
                    List<int> targetUserId = targetedUserIds.Split(',')
                                             .Select(id => int.Parse(id.Trim()))
                                             .ToList();
                    notification.TargetedUserIds = string.Join(",", targetUserId);
                }
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

        }

        public async Task<List<Notification>> ViewAllNotificationsAsync()
        {
            return _context.Notifications.ToList();
        }
        public async Task<List<Notification>> ViewNotificationsByUserIdAsync(int viewerId)
        {
            string viewerIdstr = Convert.ToString(viewerId);
            List<Notification> notifications = await _context.Notifications.ToListAsync();
            List<Notification> notificationsById = new List<Notification>();
            var targetIds = await _context.Notifications.Select(x => x.TargetedUserIds).ToListAsync();
            for (int iterator = 0; iterator < targetIds.Count; iterator++)
            {
                var targetId = targetIds[iterator].Split(",");
                if (targetId.Contains(viewerIdstr))
                {
                    if (notifications[iterator].isRead is false || notifications[iterator].isRead is null)
                    {
                        notificationsById.Add(notifications[iterator]);
                        notifications[iterator].isRead = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return notificationsById;   
        }
    }
}
