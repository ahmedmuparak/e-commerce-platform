using Ecommerce.Application.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthService 
    {
        Task<UserDTO> LogIn(LogInDTO logInDTO);
        Task<UserDTO> Register(RegisterDTO registerDTO);
        Task<bool> CheckEmail(string email);
        Task<UserDTO> GetUserByEmail(string email);
        Task<AddressDTO> GetCurrentUserAddressAsync(string userId);
        Task<bool> UpdateCurrentUserAddressAsync(string userId, AddressDTO addressDTO);
    }
}
