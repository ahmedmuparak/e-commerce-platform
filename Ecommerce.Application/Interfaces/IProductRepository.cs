using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAll(
            int? CategoryId,
            decimal? MinPrice,
            decimal? MaxPrice,
            string? Search,
            string? SortDirection,
            int PageNumber = 1,
            int PageSize = 10);
        Task<Product?> GetById(int id);
        Task<Product> Add(Product product);
        Task Update(Product product);
        Task Delete(Product product);
    }
}
