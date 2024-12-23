using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>()
                .Property(b => b.Status)
                .HasDefaultValue(OrderStatus.Pending);
        }
    }
}
