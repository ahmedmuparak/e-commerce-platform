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
        Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string userId, string email);
        Task<IEnumerable<OrderToReturnDTO>> GetUserOrders(string email);
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethods();
        Task<OrderToReturnDTO> GetOrderByIdForUser(int orderId, string email);
        Task<OrderToReturnDTO> UpdateOrderState(int orderId, OrderStatus status);
        Task<List<OrderToReturnDTO>> GetAllOrders();
        List<string> GetOrderStatuses();
        Task<OrderToReturnDTO> GetOrderById(int orderId);
    }
}
