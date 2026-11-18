using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService   // it haas four method signatures ( GetProducts, GetProductById, GetProductBrands, GetProductCategories)
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProsuctSpecsParams specParams);
        Task<Product?> GetProductByIdAsync(int ProductId);
        Task<int> GetCountAsync(ProsuctSpecsParams specParams);
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetProductCategoriesAsync();
    }
}
