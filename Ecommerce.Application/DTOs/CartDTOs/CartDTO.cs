using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.CartDTOs
{
    public class CartDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<CartItemDTO> Items { get; set; } = new();
    }
}


