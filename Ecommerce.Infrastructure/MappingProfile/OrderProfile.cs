using AutoMapper;
using Ecommerce.Application.DTOs.OrderDTOs;
using Ecommerce.Domain.Entities.OrderModule;

namespace Ecommerce.Infrastructure.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Address
            CreateMap<OrderAddressDTO, OrderAddress>().ReverseMap();

            // Delivery Method
            CreateMap<DeliveryMethod, DeliveryMethodDTO>();

            // Order Item
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.IMG,
                    o => o.MapFrom(s => s.Product != null ? s.Product.IMG : null))
                .ForMember(d => d.ProductName,
                    o => o.MapFrom(s => s.Product.ProductName));

            // Order
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod,
                    o => o.MapFrom(s => s.DeliveryMethod))
                .ForMember(d => d.items,
                    o => o.MapFrom(s => s.Items))
                .ForMember(d => d.Address,
                    o => o.MapFrom(s => s.address))
                .ForMember(s=>s.OrderDate,
                    o =>o.MapFrom(s=>s.OrderDate))
                .ForMember(s => s.UserEmail,
                    o => o.MapFrom(s => s.UserEmail))
                .ForMember(s => s.OrderStatus,
                    o => o.MapFrom(s => s.OrderStatus))
                .ForMember(d => d.Total,
                    o => o.MapFrom(s => s.SubTotal + (s.DeliveryMethod != null ? s.DeliveryMethod.Price : 0)));
        }
    }
}