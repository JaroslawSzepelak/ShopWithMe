using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.Products;
using ShopWithMe.Models.Storage;
using ShopWithMe.Tools.Abstractions;
using ShopWithMe.Tools.Services;

namespace ShopWithMe.Models
{
    public class DefaultContext : DbContext
    {
        private UserService _userSevice;

        #region DefaultContext()
        public DefaultContext(DbContextOptions<DefaultContext> options, UserService userSevice) 
            : base(options) 
        { 
            _userSevice = userSevice;
        }
        #endregion

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StorageFile> Files { get; set; }

        #region OnModelCreating()
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Oreder
            modelBuilder.Entity<Order>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>()
                .Property(b => b.Status)
                .HasDefaultValue(OrderStatus.Pending);
            #endregion

            #region StorageFile
            modelBuilder.Entity<StorageFile>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<StorageFile>()
                .Property(b => b.DateModified)
                .HasDefaultValueSql("getdate()");
            #endregion
        }
        #endregion

        #region SaveChanges()
        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries();

            foreach (var changedEntity in changedEntities)
            {
                #region IAuditable
                if (changedEntity.Entity is IAuditable)
                {
                    var entity = (IAuditable)changedEntity.Entity;

                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.DateCreated = DateTime.Now;
                            entity.CreatedBy = _userSevice.GetUserId();
                            entity.DateModified = DateTime.Now;
                            entity.ModifiedBy = _userSevice.GetUserId();
                            break;

                        case EntityState.Modified:
                            entity.DateModified = DateTime.Now;
                            entity.ModifiedBy = _userSevice.GetUserId();
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region BaseEntity
                if (changedEntity.Entity is BaseEntity)
                {
                    var entity = (BaseEntity)changedEntity.Entity;

                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.OnBeforeInsert();
                            break;

                        case EntityState.Modified:
                            entity.OnBeforeUpdate();
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }

            return base.SaveChanges();
        }
        #endregion
    }
}
