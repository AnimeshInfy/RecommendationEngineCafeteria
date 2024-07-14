using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> ViewAllNotificationsAsync();
        Task SendNotification(string message);
        Task<List<Notification>> ViewNotificationsByUserIdAsync(int viewerId);

    }
}
