using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.FavoriteDTOs
{
    public class FavoriteListDTO
    {
        public int UserId {  get; set; }
        public List<FavoriteItemDTO> Items { get; set; }
    }
}

