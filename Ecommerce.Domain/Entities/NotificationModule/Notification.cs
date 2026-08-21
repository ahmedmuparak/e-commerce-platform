using Ecommerce.Domain.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.NotificationModule
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
    }
}
