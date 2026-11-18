using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using talabat.apis.Errors;
using talabat.apis.Helpres;
using talabat.apis.Middelwares;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Services;

namespace Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services (add services to the DI container)

            // ---------- Controllers & JSON ----------
            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // ---------- Swagger ----------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ---------- Databases ----------
            // Store database (connection string: DefaultConnection in appsettings.json)
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                // options.UseLazyLoadingProxies();
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Identity (security) database
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // ---------- Redis ----------
            builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var redisConnection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConnection);
            });

            // ---------- Repositories ---------- (allow Dependency Injection)
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

            // ---------- AutoMapper ----------
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = "YOUR_LICENSE_KEY";
            }, typeof(MappingProfiles).Assembly);

            // ---------- API behavior (custom model validation error response) ----------
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = errors
                    });
                };
            });

            // ---------- Identity ----------
            builder.Services
                .AddIdentity<AppUser, IdentityRole>(options =>
                {
                    //options.Password.RequireDigit = true;
                    //options.Password.RequireLowercase = true;
                    //options.Password.RequireUppercase = true;
                    //options.Password.RequireNonAlphanumeric = false;
                    //options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            #endregion

            var app = builder.Build();

            #region Apply migrations & seed data on startup

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var storeContext = services.GetRequiredService<StoreContext>();
                    await storeContext.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(storeContext);

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                }
                catch (Exception ex)
                {
                    loggerFactory.CreateLogger<Program>()
                        .LogError(ex, "An error occurred during migration.");
                }
            }

            #endregion

            #region Configure the HTTP request pipeline (middlewares)

            app.UseMiddleware<ExceptionMiddelware>();           // global exception handling

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // global status-code error handling

            app.UseHttpsRedirection();
            app.UseStaticFiles();                               // serve files from wwwroot
            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}