using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly EcommerceDbContext _context;

        public FavoriteRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteList>> GetUserFavorites(string userId)
        {
            return await _context.FavoriteLists
                .Include(f => f.Product)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task AddToFavorites(string userId, int productId)
        {
            var exists = await _context.FavoriteLists
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

            if (exists) return;

            var favorite = new FavoriteList
            {
                UserId = userId,
                ProductId = productId
            };

            _context.FavoriteLists.Add(favorite);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromFavorites(string userId, int productId)
        {
            var fav = await _context.FavoriteLists
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (fav == null) return;

            _context.FavoriteLists.Remove(fav);
            await _context.SaveChangesAsync();
        }
    }
}