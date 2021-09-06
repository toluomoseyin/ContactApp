using ContactBookApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Data
{
    public static  class SeedDatabase
    {
        public static async Task SeedData(ContactBookDbContext context, UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();
            await SeedRole(roleManager);
            await SeedUser(userManager, context);
        }


        public static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.RoleExistsAsync("Regular").Result == false)
            {
                var role = new IdentityRole
                {
                    Name = "Regular",
                   
                    
                };
                await roleManager.CreateAsync(role);
            }

            if (roleManager.RoleExistsAsync("Admin").Result == false)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(role);
            }
        }


        private static async Task SeedUser(UserManager<IdentityUser> userManager, ContactBookDbContext context)
        {
            if (userManager.FindByEmailAsync("tolu@gmail.com").Result == null)
            {
                var adminUser = new IdentityUser
                {
                    Email = "tolu@gmail.com",
                    UserName = "tolu@gmail.com",
                                           
                    
                };
                IdentityResult result = userManager.CreateAsync(adminUser, "Tolu123@").Result;
                if (result.Succeeded)
                {
                    await context.SaveChangesAsync(); userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
               


            }
           
        }

    }

}
