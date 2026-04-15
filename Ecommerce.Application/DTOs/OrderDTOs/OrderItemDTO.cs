using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public string ProductName { get; set; } 
        public string IMG { get; set; }  
        public int Quantity { get; set; }
        public decimal Price { get; set; }        
    }
}

