using Ecommerce.Domain.Entities.CartModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface ICartRepository
    { 
        Task<Cart?> GetUserCart(string userId);

        Task AddItemAsync(string userId, int productId);
        Task UpdateItemQuantityAsync(int cartItemId, int quantity);

        Task RemoveItemAsync(int cartItemId);

        Task ClearCartAsync(string userId);
    }
}

