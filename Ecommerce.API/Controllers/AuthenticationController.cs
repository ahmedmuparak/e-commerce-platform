using Ecommerce.Application.DTOs.AuthDTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authenticationService;

        public AuthenticationController(IAuthService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LogIn(LogInDTO logInDTO)
        {
            try
            {
                var result = await authenticationService.LogIn(logInDTO);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            try
            {
                var result = await authenticationService.Register(registerDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result = await authenticationService.CheckEmail(Email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email =  User.FindFirstValue(ClaimTypes.Email);
            var Result = await authenticationService.GetUserByEmail(Email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = await authenticationService.GetCurrentUserAddressAsync(userId);

            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<IActionResult> UpdateAddress(AddressDTO addressDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var result = await authenticationService
                .UpdateCurrentUserAddressAsync(userId, addressDto);

            return Ok(result);
        }
    }
}