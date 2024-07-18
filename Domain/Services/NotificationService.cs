using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.ModelDTO;
using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository; 
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUserRepository _userRepository;
        public NotificationService(INotificationRepository notificationRepository, 
            IMenuItemRepository menuItemRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _menuItemRepository = menuItemRepository;
            _userRepository = userRepository;
        }

        public async Task GetDetailedFeedbackOnDiscardedItems(int foodId)
        {
            var foodName = _menuItemRepository.GetFoodItemNameById(foodId).Result[0];
            bool isFoodItemDiscarded = _menuItemRepository.isFoodItemUnderDiscardMenu(foodId);
            if (isFoodItemDiscarded)
            {
                string message = $"\n1.What didn’t you like about {foodName}? " +
                $"\nQ2.How would you like {foodName} to taste? " +
                "\nQ3.Share your mom’s recipe\n";
                string sendToAllUsers = "Y";
                string sendNotificationrequest = $"{message}:{sendToAllUsers}";

                await _notificationRepository.SendNotification(sendNotificationrequest);
            }
        }

        public async Task GiveDetailedFeedbackOnDiscardedItems(string message)
        {
            var receiverIds = _userRepository.GetAdminAndChefUserId();
            string sendToAllUsers = "N";
            string sendNotificationrequest = $"{message}:{sendToAllUsers}-{receiverIds}";
            await _notificationRepository.SendNotification(sendNotificationrequest);
        }

        public async Task SendNotification(string message)
        {
            await _notificationRepository.SendNotification(message);
        }

        public async Task<List<NotificationDTO>> ViewAllNotificationsAsync()
        {
            IEnumerable<Notification> notifications = await _notificationRepository.ViewAllNotificationsAsync();
            return notifications.Select(model => MapNotificationModelToDto(model)).ToList();
        }

        public async Task<List<NotificationDTO>> ViewNotificationsByUserIdAsync(int viewerId)
        {
            IEnumerable<Notification> notifications = await _notificationRepository.ViewNotificationsByUserIdAsync(viewerId);
            return notifications.Select(model => MapNotificationModelToDto(model)).ToList();
        }

        private NotificationDTO MapNotificationModelToDto(Notification notification)
        {
            return new NotificationDTO
            {
                Message = notification.Message,
                NotificationDate = notification.NotificationDate,
            };
        }
    }

}
