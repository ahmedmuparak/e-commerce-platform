using Ecommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.IdentityData.DataSeed
{
    public static class IdentitySeed
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));
        }


        public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            var email = "admin@store.com";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    DisplayName = "Store Admin",
                    UserName = "admin",
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin@123");

                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}

