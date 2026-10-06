using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Talabat.Core.Entites;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync( StoreContext _dbContext)
        {
            if ( _dbContext.ProductBrands.Count() == 0) // check if the ProductBrands database is empty
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json"); // read the data from json file
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData); // transfer the data from json file to list of ProductBrand 

                if (brands is not null && brands.Count > 0)
                {
                    foreach (var item in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(item); // add the data to the database
                    }
                    await _dbContext.SaveChangesAsync(); // save the changes to the database once all the data is added
                }
            }









            if (_dbContext.ProductCategories.Count() == 0) // check if the ProductCategories database is empty
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories is not null && categories.Count > 0)
                {
                    foreach (var item in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if(_dbContext.Products.Count() == 0) // check if the Products database is empty
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products is not null && products.Count > 0)
                {
                    foreach (var item in products)
                    {
                        _dbContext.Set<Product>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

