using Microsoft.EntityFrameworkCore;
using ShopWithMe.Models;
using ShopWithMe.Models.Orders;
using ShopWithMe.Models.Seed;
using ShopWithMe.Session.Models;
using SportsStore.Models;
using System.Text.Json.Serialization;

namespace ShopWithMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddDbContext<DefaultContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Configure session with appropriate settings
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(builder.Configuration.GetValue<int>("Session:Timeout"));
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // Wymagane dla po��cze� cross-origin
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // U�ywaj Always, je�li korzystasz z HTTPS
            });

            // Set up session cart and other scoped services
            builder.Services.AddScoped(sp => SessionCart.GetCart(sp));
            builder.Services.AddScoped(sp => SessionContactData.GetContactData(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

            // Configure CORS policy to allow frontend access
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:8080") // Frontend URL
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Umo�liwienie przesy�ania ciasteczek
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS with the specified policy
            app.UseCors("AllowFrontend");

            app.UseAuthorization();
            app.UseSession(); // Musi by� po `UseCors` i przed `MapControllers`

            app.MapControllers();

            // Seed database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedData.EnsurePopulated(services); // Przekazujemy IServiceProvider do SeedData
            }

            app.Run();
        }
    }
}
