using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities.NotificationModule;
using Ecommerce.Infrastructure.Hubs;
using Ecommerce.Infrastructure.IdentityData;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly EcommerceDbContext context;
        private readonly StoreidentityDBContext identity;
        private readonly IHubContext<NotificationHub> _hubContext;


        public NotificationService(EcommerceDbContext context, StoreidentityDBContext identity, IHubContext<NotificationHub> hubContext)
        {
            this.context = context;
            this.identity = identity;
            _hubContext = hubContext;
        }

        public async Task CreateNotification(Notification notification)
        {
            var not = new Notification
            {
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = false,
                Created = DateTime.UtcNow,
                Type = notification.Type
            };
            context.Notifications.Add(not);

        }

        public async Task<List<Notification>> GetNotificationForUser(string userid)
        {
            var userExists = await identity.Users
                .AnyAsync(u => u.Id == userid);

            if (!userExists)
                throw new Exception("User not found");

            var notifications = await context.Notifications
                .Where(n => n.UserId == userid)
                .OrderByDescending(n => n.Created)
                .ToListAsync();

            return notifications;
        }

        public async Task MarkNotificationAsRead(int notificationId)
        {
            var notification = await context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification == null)
                throw new Exception("Notification not found");
            notification.IsRead = true;
            await context.SaveChangesAsync();
        }

        public  async Task<List<Notification>> GetunreadNotifications(string userId)
        {
            var UnreadNotifications = await context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
            if(UnreadNotifications == null)
                throw new Exception("No unread notifications found");   

            return UnreadNotifications;
        }

        public async Task<int> GetUnreadcount(string userId)
        {
            var Unreadcount = await context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);

            return Unreadcount;
        }

        public async Task<Notification> GetNotificationById(int notificationId ,string userId)
        {
            var notification = await context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
                throw new Exception("Notification not found");
            notification.IsRead = true;

            return notification;
        }

        public async Task SendNotificationAsync(
    string userId,
    Notification notification)
        {
            await _hubContext
                .Clients
                .Group($"User_{userId}")
                .SendAsync(
                    "ReceiveNotification",
                    notification
                );
        }
    }
}