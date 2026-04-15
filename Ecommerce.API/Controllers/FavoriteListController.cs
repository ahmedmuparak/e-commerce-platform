using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.FavoriteDTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteListController : ControllerBase
    {
        private readonly IFavoriteRepository repo;

        public FavoriteListController(IFavoriteRepository repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddFavoriteDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await repo.AddToFavorites(userId, dto.ProductId);

            return Ok("Product added to favorites");
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await repo.RemoveFromFavorites(userId, productId);

            return Ok("Product removed from favorites");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = await repo.GetUserFavorites(userId);

            var result = products.Select(p => new FavoriteItemDTO
            {
                Name = p.Product.Name,
                Price = p.Product.Price,
                Description = p.Product.Description,
                IMG = p.Product.ImageUrl
            });

            return Ok(result);
        }
    }
}