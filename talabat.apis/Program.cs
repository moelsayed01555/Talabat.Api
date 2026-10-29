
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;
using talabat.apis.Errors;
using talabat.apis.Helpres;
using talabat.apis.Middelwares;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region // Configure Services (Add services to the container).

            webApplicationBuilder.Services.AddControllers();
            //Register the required web apis services into the DI Container

            //allow DI for Swagger
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            // webApplicationBuilder.Services.AddOpenApi();

            // allow DI for the database context
            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>       // use options because of parameterized constructor in StoreContext class
            {                                                                          //Connection string (name : DefaultConnection) is stored in appsettings.json file
                //options.UseLazyLoadingProxies();  // enable lazy loading for navigation properties in the entities
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // allow DI for Security database 
            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(options =>       
            {                                                                         
               
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // allow DI for the redis context


            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>( (ServiceProvider) =>
            {
                var redisConnection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConnection);
                });
            // allow DI for the repository
            #region بدل كل ده هننفز اللي تحتها
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>,GenericRepository<ProductBrand>>();
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductCategory>,GenericRepository<ProductCategory>>();
            #endregion

            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // allow DI for all entities applying Igeneric Reopsitory

            webApplicationBuilder.Services.AddScoped(typeof(IBasketRepository) , typeof(BasketRepository));     // allow DI for Basket Repository

            webApplicationBuilder.Services.AddControllers().AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                   });

            webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));  // allow DI for automapper


            // Configure the API behavior for model validation errors
            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            #endregion


            var app = webApplicationBuilder.Build();

            #region Make automatic migration instead of writing update-database command in package manager console
            // i need to make app do migration automatically when the application starts
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;                          // ask for object from scoped services( di container) 
            var _dbContext = services.GetRequiredService<StoreContext>();  // ask clr to create object from dbContext
             
            var loggerFactory = services.GetRequiredService<ILoggerFactory>(); // ask clr to create object from loggerFactory to show error in obvious way
            try
            {
                await _dbContext.Database.MigrateAsync();  // Perform automatic migration( update database)
                await StoreContextSeed.SeedAsync(_dbContext); // Seed the database with initial data
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during migration
                loggerFactory.CreateLogger<Program>().LogError(ex, "An error occurred during migration.");  // write the error in the log file
            }
            #endregion

            #region Configure kestrel Middelwares
            app.UseMiddleware<ExceptionMiddelware>();  // to handle the exceptions in a global way

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
               // app.MapOpenApi();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");     // to handle the status code errors in a global way

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();  // to allow the application to serve static files (images, css, js, etc.) from the wwwroot folder


            app.MapControllers();  // is a middelware for routing
            #endregion


            app.Run();
        }
    }
}
