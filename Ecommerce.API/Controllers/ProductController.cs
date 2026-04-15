using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repo;

        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? CategoryId, decimal? MinPrice, decimal? MaxPrice, string? Search , string? SortDirection)
        {
            var products = await repo.GetAll(CategoryId, MinPrice, MaxPrice,Search, SortDirection);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await repo.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Authorize (Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePaths = new List<string>();

            if (dto.ImageUrl != null && dto.ImageUrl.Count > 0)
            {
                foreach (var image in dto.ImageUrl)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    var path = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePaths.Add(fileName);
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                ImageUrl = string.Join(",", imagePaths)
            };

            await repo.Add(product);

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await repo.GetById(id);

            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            if (dto.ImageUrl != null && dto.ImageUrl.Count > 0)
            {
                var imageNames = new List<string>();

                foreach (var image in dto.ImageUrl)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    var path = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imageNames.Add(fileName);
                }

                product.ImageUrl = string.Join(",", imageNames);
            }

            await repo.Update(product);

            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await repo.GetById(id);

            if (product == null)
                return NotFound();

            await repo.Delete(product);

            return NoContent();
        }
    }
}
