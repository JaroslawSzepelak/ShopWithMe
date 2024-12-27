using ShopWithMe.Managers.Orders;
using ShopWithMe.Managers.ProductCategories;
using ShopWithMe.Managers.Products;
using ShopWithMe.Managers.Storage;

namespace ShopWithMe.Managers
{
    public static class DependencyInjection
    {
        #region AddManagers()
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services = services.AddScoped<ProductsManager>();
            services = services.AddScoped<ProductCategoriesManager>();
            services = services.AddScoped<OrdersManager>();
            services = services.AddScoped<StorageManager>();
            services = services.AddScoped<ProductImagesManager>();

            return services;
        }
        #endregion
    }
}
