using backend.Models; 
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                string[] roleNames = { UserRoles.Admin, UserRoles.Barber, UserRoles.User };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                        logger.LogInformation("Seeding: Role {Role} byla úspěšně vytvořena.", roleName);
                    }
                }
                var adminEmail = configuration["SeedSettings:AdminEmail"];

                var adminUser = await userManager.FindByEmailAsync(adminEmail!);

                if (adminUser != null)
                {
                    if (!await userManager.IsInRoleAsync(adminUser, UserRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                        logger.LogInformation("Seeding: Uživateli {Email} byla udělena role Admin.", adminEmail);
                    }
                }
                else
                {
                    logger.LogWarning("Seeding: Uživatel s e-mailem {Email} nebyl nalezen. " +
                                     "Zaregistruj se prosím, aby ti mohla být role udělena.", adminEmail);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Při seedování databáze došlo k chybě.");
                throw; 
            }
        }
    }
}