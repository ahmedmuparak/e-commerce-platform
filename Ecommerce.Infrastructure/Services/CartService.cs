using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.IdentityData;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo; 
        private readonly StoreidentityDBContext _identity;

        public CartService(ICartRepository cartRepo,
                           StoreidentityDBContext identity)
        {
            _cartRepo = cartRepo;
            _identity = identity;
        }

        public async Task<CartDTO?> GetCartAsync(string userId)
        {
            var userExists = await _identity.Users 
                .AnyAsync(u => u.Id == userId);

            if (!userExists)
                throw new Exception("User not found");

            var cart = await _cartRepo.GetUserCart(userId);

            if (cart == null)
                return null;

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductImage = i.Product.ImageUrl,
                    ProductPrice = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task AddItemAsync(string userId, int productId)
        {
            await _cartRepo.AddItemAsync(userId, productId);
        }
        public async Task UpdateItemQuantityAsync(int cartItemId, int quantity)
        {
            await _cartRepo.UpdateItemQuantityAsync(cartItemId, quantity);
        }
        public async Task RemoveItemAsync(int cartItemId)
        {
            await _cartRepo.RemoveItemAsync(cartItemId);
        }

        public async Task ClearCartAsync(string userId)
        {
            await _cartRepo.ClearCartAsync(userId);
        }
    }
}