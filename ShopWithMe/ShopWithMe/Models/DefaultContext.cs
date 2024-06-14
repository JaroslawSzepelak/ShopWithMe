using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
