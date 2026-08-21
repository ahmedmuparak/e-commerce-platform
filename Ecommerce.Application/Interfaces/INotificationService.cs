using Ecommerce.Domain.Entities.NotificationModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotification(Notification notification);
        Task<List<Notification>> GetNotificationForUser(string userid);
        Task MarkNotificationAsRead(int notificationId);
        Task<List<Notification>> GetunreadNotifications(string userId);
        Task<int> GetUnreadcount(string userId);
        Task<Notification> GetNotificationById(int notificationId, string userId);
        Task SendNotificationAsync(string userId, Notification notification);
    }
}
