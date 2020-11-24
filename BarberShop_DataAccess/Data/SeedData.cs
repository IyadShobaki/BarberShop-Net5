using BarberShop_Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarberShop_DataAccess.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }
        private async static Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@barbershop.com") == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "BarberShop",
                    PhoneNumber = "1112223333",
                    UserName = "admin@barbershop.com",
                    Email = "admin@barbershop.com"
                };
                var result = await userManager.CreateAsync(user, "Pwd12345.");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            //if (!roleManager.RoleExistsAsync("Administrator").Result)
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
               // var result = roleManager.CreateAsync(role).Result;
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
