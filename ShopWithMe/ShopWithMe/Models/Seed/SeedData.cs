using Microsoft.EntityFrameworkCore;

namespace ShopWithMe.Models.Seed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            DefaultContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DefaultContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            context.SaveChanges();
        }
    }
}
