using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.OrderModule
{
    public class OrderItem : BaseEntity
    {
        public ProductItemOrder Product { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

