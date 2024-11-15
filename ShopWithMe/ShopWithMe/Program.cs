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

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(builder.Configuration.GetValue<int>("Session:Timeout"));
                options.Cookie.HttpOnly = true;
            });
            builder.Services.AddScoped(sp => SessionCart.GetCart(sp));
            builder.Services.AddScoped(sp => SessionContactData.GetContactData(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:8080") // URL frontendu
                        .AllowAnyHeader()
                        .AllowAnyMethod();
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
            app.UseSession();

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
