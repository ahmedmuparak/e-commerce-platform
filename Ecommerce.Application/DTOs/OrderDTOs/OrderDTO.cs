using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int DeliveryMethodID { get; set; }
        public OrderAddressDTO Address { get; set; }
    }
}

