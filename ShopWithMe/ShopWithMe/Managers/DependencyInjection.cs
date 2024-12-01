namespace ShopWithMe.Managers
{
    public static class DependencyInjection
    {
        #region AddManagers()
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services = services.AddScoped<ProductsManager>();
            services = services.AddScoped<ProductCategoriesManager>();

            return services;
        }
        #endregion
    }
}
