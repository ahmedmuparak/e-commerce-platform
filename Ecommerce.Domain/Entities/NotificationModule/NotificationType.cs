using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.NotificationModule
{
    public enum NotificationType
    {
        OrderCreated,
        OrderStatusChanged,
        ProductBackInStock,
        System
    }
}
