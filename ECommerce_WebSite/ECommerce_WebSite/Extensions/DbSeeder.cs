using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_WebSite.Extensions
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // 1. Resolve RoleManager and UserManager from the DI Container
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<App_User>>();

            // 2. Define the roles we want in our system
            string[] roleNames = { "Admin", "Customer" };

            // 3. Loop through and create them if they don't exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 4. Assign the Admin role to your specific email
            string adminEmail = "abdohalem305@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser != null)
            {
                // Check if the user already has the Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    // Assign the Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
