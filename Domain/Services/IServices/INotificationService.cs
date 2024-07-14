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
        Task<List<Notification>> ViewNotificationsByUserIdAsync();
        Task<List<Notification>> ViewAllNotificationsAsync();
        Task SendNotification(string message);
        Task GetDetailedFeedbackOnDiscardedItems();
    }
}
