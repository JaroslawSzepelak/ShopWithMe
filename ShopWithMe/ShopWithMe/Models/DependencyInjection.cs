using ShopWithMe.Models.ProductCategories;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Models
{
    public static class DependencyInjection
    {
        #region AddMappers()
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services = services.AddScoped<ProductModelsMapper>();
            services = services.AddScoped<ProductCategoryModelsMapper>();

            return services;
        }
        #endregion
    }
}
