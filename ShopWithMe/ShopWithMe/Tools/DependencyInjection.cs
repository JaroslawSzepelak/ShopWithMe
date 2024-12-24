using ShopWithMe.Tools.Services;

namespace ShopWithMe.Tools
{
    public static class DependencyInjection
    {
        #region AddMappers()
        public static IServiceCollection AddTools(this IServiceCollection services)
        {
            services = services.AddScoped<UserService>();

            return services;
        }
        #endregion
    }
}
