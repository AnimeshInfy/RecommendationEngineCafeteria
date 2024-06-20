using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> ViewAllNotificationsAsync();
        void SendNotification();
    }
}
