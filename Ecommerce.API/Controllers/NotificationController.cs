using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications =
                await notificationService.GetNotificationForUser(userId);

            return Ok(notifications);
        }

        [Authorize]
        [HttpPost("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            await notificationService.MarkNotificationAsRead(notificationId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("unreadNotifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var unreadNotifications = await notificationService.GetunreadNotifications(userId);
            return Ok(unreadNotifications);
        }

        [Authorize]
        [HttpGet("unreadNotificationsCount")]
        public Task<int> GetUnreadcount()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UnreadCount = notificationService.GetUnreadcount(userId);

            return UnreadCount;
        }

        [Authorize]
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = await notificationService.GetNotificationById(notificationId, userId);
            return Ok (notification);
        }
    }
}
