using Data.ModelDTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> ViewNotificationsByUserIdAsync(int viewerId);
        Task<List<NotificationDTO>> ViewAllNotificationsAsync();
        Task SendNotification(string message);
        Task GetDetailedFeedbackOnDiscardedItems(int foodId);
        Task GiveDetailedFeedbackOnDiscardedItems(string message);
    }
}
