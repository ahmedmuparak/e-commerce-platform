using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.CartModule
{
    public class CartItem:BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; } = 1;

        public int CartId { get; set; }

        public Cart Cart { get; set; }
    }
}

