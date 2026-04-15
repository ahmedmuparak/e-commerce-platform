using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public ICollection<OrderItemDTO> items { get; set; }
        public OrderAddressDTO Address { get; set; }
        public string DeliveryMethod { get; set; }
        public string OrderStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
