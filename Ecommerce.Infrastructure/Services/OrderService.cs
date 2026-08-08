using AutoMapper;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.CartModule;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Infrastructure.IdentityData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly EcommerceDbContext context;
        private readonly StoreidentityDBContext identity;

        public OrderService(EcommerceDbContext context ,StoreidentityDBContext identity)
        {
            this.context = context;
            this.identity = identity;
        }

        public async Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string userId, string email)
        {
            var userExists = await identity.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new Exception("User not found");

            var cart = await context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                throw new Exception("Cart is empty");

            var deliveryMethod = await context.DeliveryMethods
                .FirstOrDefaultAsync(d => d.Id == orderDTO.DeliveryMethodID);

            if (deliveryMethod == null)
                throw new Exception("Delivery Method Not Found");

            var address = new OrderAddress
            {
                FirstName = orderDTO.Address.FirstName,
                LastName = orderDTO.Address.LastName,
                City = orderDTO.Address.City,
                Street = orderDTO.Address.Street,
                Address = orderDTO.Address.Address,
                Country = orderDTO.Address.Country
            };

            var orderItems = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                orderItems.Add(new OrderItem
                {
                    Product = new ProductItemOrder
                    {
                        ProductId = item.Product.Id,
                        ProductName = item.Product.Name,
                        IMG = item.Product.ImageUrl
                    },
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            var order = new Order
            {
                UserEmail = email,
                address = address,
                DeliveryMethodId = deliveryMethod.Id,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = orderItems.Sum(i => i.Price * i.Quantity)
            };

            context.Orders.Add(order);

            context.Carts.Remove(cart);

            await context.SaveChangesAsync();

            return new OrderToReturnDTO
            {
                Id = order.Id,
                UserEmail = order.UserEmail,
                Address = new OrderAddressDTO
                {
                    FirstName = order.address.FirstName,
                    LastName = order.address.LastName,
                    City = order.address.City,
                    Street = order.address.Street,
                    Address = order.address.Address,
                    Country = order.address.Country
                },
                DeliveryMethod = order.DeliveryMethod.ShortName,
                OrderStatus = order.OrderStatus.ToString(),
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),

                items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.ProductName,
                    IMG = i.Product.IMG,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderToReturnDTO>> GetUserOrders(string email)
        {
            var orders = await context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.DeliveryMethod)
                .Include(o => o.address)
                .Where(o => o.UserEmail == email)
                .ToListAsync();

            if (!orders.Any())
                throw new Exception("No orders found");

            return orders.Select(order => new OrderToReturnDTO
            {
                Id = order.Id,
                UserEmail = order.UserEmail,

                Address = new OrderAddressDTO
                {
                    FirstName = order.address.FirstName,
                    LastName = order.address.LastName,
                    City = order.address.City,
                    Street = order.address.Street,
                    Address = order.address.Address,
                    Country = order.address.Country
                },

                DeliveryMethod = order.DeliveryMethod.ShortName,
                OrderStatus = order.OrderStatus.ToString(),
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),

                items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.ProductName,
                    IMG = i.Product.IMG,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });
        }

        public async Task<OrderToReturnDTO> GetOrderByIdForUser(int orderId, string email)
        {
            var User = identity.Users.FirstOrDefault(u => u.Email == email);

            if (User == null)
                throw new Exception("User not found");

            var order = context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.DeliveryMethod)
                .Include(o => o.address)
                .FirstOrDefault(o => o.Id == orderId && o.UserEmail == email);

            if (order == null)
                throw new Exception("Order not found");

            return new OrderToReturnDTO
            {
                Id = order.Id,
                UserEmail = order.UserEmail,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryMethod = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),

                Address = new OrderAddressDTO
                {
                    FirstName = order.address.FirstName,
                    LastName = order.address.LastName,
                    City = order.address.City,
                    Street = order.address.Street,
                    Address = order.address.Address,
                    Country = order.address.Country
                },

                items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.ProductName,
                    IMG = i.Product.IMG,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        public async Task<List<OrderToReturnDTO>> GetAllOrders()
        {
            var orders = await context.Orders
    .Include(o => o.Items)
        .ThenInclude(i => i.Product)
    .Include(o => o.DeliveryMethod)
    .Include(o => o.address)
    .ToListAsync();

            return orders.Select(order => new OrderToReturnDTO
            {
                Id = order.Id,
                UserEmail = order.UserEmail,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryMethod = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                Address = new OrderAddressDTO
                {
                    FirstName = order.address.FirstName,
                    LastName = order.address.LastName,
                    City = order.address.City,
                    Street = order.address.Street,
                    Address = order.address.Address,
                    Country = order.address.Country
                },
                items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.ProductName,
                    IMG = i.Product.IMG,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            }).ToList();
        }


        public async Task<OrderToReturnDTO> UpdateOrderState(int orderId, OrderStatus status)
        {
            var order = context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.DeliveryMethod)
                .Include(o => o.address)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.OrderStatus = status;

            await context.SaveChangesAsync();

            return new OrderToReturnDTO
            {
                Id = order.Id,
                UserEmail = order.UserEmail,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryMethod = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                Total = order.GetTotal(),

                Address = new OrderAddressDTO
                {
                    FirstName = order.address.FirstName,
                    LastName = order.address.LastName,
                    City = order.address.City,
                    Street = order.address.Street,
                    Address = order.address.Address,
                    Country = order.address.Country
                },

                items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.ProductName,
                    IMG = i.Product.IMG,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }
        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await context.DeliveryMethods.ToListAsync();

            return deliveryMethods;
        }

        public List<string> GetOrderStatuses()
        {
            return Enum.GetNames<OrderStatus>().ToList();
        }
    }
}

