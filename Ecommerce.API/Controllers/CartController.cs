using Ecommerce.Application.DTOs.CartDTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null) 
                return NotFound();

            return Ok(cart);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddCartItemDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.AddItemAsync(userId, dto.ProductId);

            return Ok("Item added to cart");
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem(UpdateCartItemDTO dto)
        {
            await _cartService.UpdateItemQuantityAsync(dto.CartItemId, dto.Quantity);

            return Ok("Cart updated");
        }

        [Authorize]
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            await _cartService.RemoveItemAsync(cartItemId);

            return Ok("Item removed");
        }

        [Authorize]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.ClearCartAsync(userId);

            return Ok("Cart cleared");
        }
    }
}