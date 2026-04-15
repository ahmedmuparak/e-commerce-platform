using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces;


namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcommerceDbContext context;

        public CategoryRepository(EcommerceDbContext context)  
        {
            this.context = context;
        }

        public async Task<List<Category>> GetAll()
        {
            var Categories = await context.Categories
                .Include(c=>c.Products)
                .ToListAsync();

            return Categories;
        }

        public async Task<Category> GetById(int id)
        {
            return await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> Add(Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task Update(Category category)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

    }
}
