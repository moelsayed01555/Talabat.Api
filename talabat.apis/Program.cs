
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

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
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // allow DI for the repository
            #region بدل كل ده هننفز اللي تحتها
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>,GenericRepository<ProductBrand>>();
            //webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductCategory>,GenericRepository<ProductCategory>>();
            #endregion

            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // allow DI for the repository

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
            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
               // app.MapOpenApi();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();  // is a middelware for routing
            #endregion


            app.Run();
        }
    }
}
