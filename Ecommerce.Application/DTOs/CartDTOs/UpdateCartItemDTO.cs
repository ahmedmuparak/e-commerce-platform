using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.CartDTOs
{
    public class UpdateCartItemDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}

