using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.OrderDTOs
{
    public class OrderAddressDTO
    {
        public string FirstName {  get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Address { get; set; }
        public string Country { get; set; } = default!;
    }
}
