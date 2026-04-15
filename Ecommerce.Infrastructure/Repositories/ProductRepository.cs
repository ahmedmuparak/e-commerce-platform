using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.DTOs.ProductDTOs;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext context;

        public ProductRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll( 
            int? CategoryId,
            decimal? MinPrice,
            decimal? MaxPrice,
            string? Search,
            string? SortDirection,
            int PageNumber = 1,
            int PageSize = 10)
        {
            var query = context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == CategoryId);
            if (MinPrice.HasValue)
                query = query.Where(p => p.Price >= MinPrice.Value);
            if (MaxPrice.HasValue)
                query = query.Where(p => p.Price <= MaxPrice.Value);
            if (!string.IsNullOrEmpty(Search))
                query = query.Where(p => p.Name.Contains(Search.Trim()));
            if (!string.IsNullOrEmpty(SortDirection))
            {
                query = SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
            }
            var skip = (PageNumber - 1) * PageSize;

            return await query
                .Skip(skip)
                .Take(PageSize)
                .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
            }).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products
                .Include (p => p.Category)
                .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<Product> Add(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task Update(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
