using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<FavoriteList>> GetUserFavorites(string userId);

        Task AddToFavorites(string userId, int propertyId);

        Task RemoveFromFavorites(string userId, int propertyId);
    }
}


