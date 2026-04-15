using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO,string userId ,string email);
        Task<IEnumerable<DeliveryMethodDTO>> GetAllDeliveryMethods();
        Task<IEnumerable<OrderToReturnDTO>> GetOrdersForUser(string userId);
        Task<OrderToReturnDTO> GetOrderById(int id, string userId);
    }
}
