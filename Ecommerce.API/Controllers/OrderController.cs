using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecommerce.Domain.Entities.OrderModule;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (userId == null)
                return Unauthorized();

            var result = await orderService.CreateOrder(orderDTO, userId, email);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrderForSpecificUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await orderService.GetUserOrders(email);

            return Ok(result);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdForUser(int orderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await orderService.GetOrderByIdForUser(orderId, email);
            return Ok(result);
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await orderService.GetAllOrders();
            return Ok(result);
        }

        [HttpGet("GetOrderByIdForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderByIdForAdmin(int orderId)
        {
            var result = await orderService.GetOrderById(orderId);
            return Ok(result);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderState(int orderId, [FromBody] OrderStatus status)
        {
            var result = await orderService.UpdateOrderState(orderId, status);
            return Ok(result);
        }

        [HttpGet("GetAllDeliveryMethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await orderService.GetAllDeliveryMethods();
            return Ok(result);
        }

        [HttpGet("GetAllOrderStatus")]
        public IActionResult GetAllOrderStatus()
        {
            var result = orderService.GetOrderStatuses();

            return Ok(result);
        }
    }
}

