using AutoMapper;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.CartModule;
using Ecommerce.Domain.Entities.OrderModule;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ecommerce.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IMapper mapper, ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string userId,string email)
        {
            var address = mapper.Map<OrderAddress>(orderDTO.Address);

            var cart = await cartRepository.GetUserCart(userId);
            if (cart == null || cart.Items.Count == 0)
                throw new Exception("Cart is empty");

            var orderItems = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var product = await unitOfWork.Repository<Product>()
                    .GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                orderItems.Add(CreateOrderItem(item, product));
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync(orderDTO.DeliveryMethodID);

            if (deliveryMethod == null)
                throw new Exception("Delivery method not found");

            var subTotal = orderItems.Sum(x => x.Price * x.Quantity);

            var order = new Order
            {
                address = address,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal,
                UserEmail = email
            };

            await unitOfWork.Repository<Order>().AddAsync(order);
            var result = await unitOfWork.CompleteAsync();

            if (result <= 0)
                throw new Exception("Order creation failed");

            await cartRepository.ClearCartAsync(userId);
            return mapper.Map<OrderToReturnDTO>(order);
             
        }

        public async Task<IEnumerable<DeliveryMethodDTO>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.Repository<DeliveryMethod>()
                .GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
        }

        private static OrderItem CreateOrderItem(CartItem item, Product product)
        {
            return new OrderItem
            {
                Product = new ProductItemOrder
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    IMG = product.ImageUrl
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }


        public async Task<IEnumerable<OrderToReturnDTO>> GetOrdersForUser(string userId)
        {
            var Orders = await unitOfWork.Repository<Order>().GetAllAsync();

            return mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
        }

        public async Task<OrderToReturnDTO> GetOrderById(int id , string userId)
        {
            var Order = await unitOfWork.Repository<Order>().GetByIdAsync(id);

            if (Order == null)
                throw new Exception($"Order not found Order with id {id} Not Found ");

            return mapper.Map<OrderToReturnDTO>(Order);
        }
    }
}

