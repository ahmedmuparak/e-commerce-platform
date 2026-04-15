using Ecommerce.Application.DTOs.AuthDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.IdentityData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.Application.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly StoreidentityDBContext context;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration , StoreidentityDBContext context)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }


        public async Task<UserDTO> LogIn(LogInDTO logInDTO)
        {
            var user = await userManager.FindByEmailAsync(logInDTO.Email);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid Credentials: User not found.");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, logInDTO.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid Credentials: Password incorrect.");

            var token = await Createtoken(user);

            return new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            };
        }
        public async Task<UserDTO> Register(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName
            };
            var IdentityResult = await userManager.CreateAsync(User, registerDTO.Password);
            var Token = await Createtoken(User);
            if (IdentityResult.Succeeded)
                return new UserDTO()
                {
                    Email = User.Email,
                    DisplayName = User.DisplayName,
                    Token = Token
                };
            throw new Exception("Registration Failed");
        }
        private async Task<string> Createtoken(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = configuration["JWTOptions:Key"];
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Cred = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: configuration["JWTOptions:Issuer"],
                audience: configuration["JWTOptions:Audience"],
                claims: Claims,
                signingCredentials: Cred,
                expires: DateTime.UtcNow.AddHours(1));

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<bool> CheckEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var User = await userManager.FindByEmailAsync(email);
            if(User == null)
                throw new UnauthorizedAccessException($"No User with Email{email}: User not found.");
            return new UserDTO
            {
                Email = User.Email,
                DisplayName = User.DisplayName,
                Token = await Createtoken(User)
            };
        }

        public async Task<AddressDTO> GetCurrentUserAddressAsync(string userId)
        {
            var address = await context.Addresses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (address == null) return null;

            return new AddressDTO
            {
                Street = address.Street,
                City = address.City,
                Country = address.Country, 
            };
        }

        public async Task<bool> UpdateCurrentUserAddressAsync(string userId, AddressDTO dto)
        {
            var address = await context.Addresses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (address == null)
            {
                address = new Address
                {
                    UserId = userId
                };

                await context.Addresses.AddAsync(address);
            }


            address.Street = dto.Street;
            address.City = dto.City;
            address.Country = dto.Country;

            await context.SaveChangesAsync();

            return true;
        }

    }
}
