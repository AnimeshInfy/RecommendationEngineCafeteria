using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;

namespace Domain.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository; 
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task GetDetailedFeedbackOnDiscardedItems(int foodId)
        {
            throw new NotImplementedException();
        }

        public async Task SendNotification(string message)
        {
            await _notificationRepository.SendNotification(message);
        }

        public async Task<List<Notification>> ViewAllNotificationsAsync()
        {
            return await _notificationRepository.ViewAllNotificationsAsync();
        }

        public async Task<List<Notification>> ViewNotificationsByUserIdAsync(int viewerId)
        {
            return await _notificationRepository.ViewNotificationsByUserIdAsync(viewerId);
        }
    }

}
