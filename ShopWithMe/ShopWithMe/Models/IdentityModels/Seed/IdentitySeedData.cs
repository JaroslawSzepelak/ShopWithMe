using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopWithMe.Models.Admin.Seed
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using AppIdentityDbContext context = scope.ServiceProvider
                    .GetRequiredService<AppIdentityDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                // Dodanie ról użytkowników
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(Roles.Administrator))
                {
                    var role = new IdentityRole
                    {
                        Name = Roles.Administrator
                    };
                    await roleManager.CreateAsync(role);
                }

                if (!await roleManager.RoleExistsAsync(Roles.Client))
                {
                    var role = new IdentityRole
                    {
                        Name = Roles.Client
                    };
                    await roleManager.CreateAsync(role);
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                // Dodanie konta administratora
                UserManager<IdentityUser> userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

                IdentityUser user = await userManager.FindByNameAsync(adminUser);

                if (user == null)
                {
                    user = new IdentityUser("Admin");
                    await userManager.CreateAsync(user, adminPassword);
                }

                if (!await userManager.IsInRoleAsync(user, Roles.Administrator))
                {
                    await userManager.AddToRoleAsync(user, Roles.Administrator);
                }
            }
        }
    }
}
