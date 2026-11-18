using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Services
{
    public class ProductService : IProductService         // serrvice deals with IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProsuctSpecsParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllSpecAsync(spec);

            return products;
        }


        public async Task<Product?> GetProductByIdAsync(int ProductId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(ProductId);
            var product = await _unitOfWork.Repository<Product>().GetSpecAsync(spec);

            return product;
        }

        public async Task<int> GetCountAsync(ProsuctSpecsParams specParams)
        {
            var countSpec = new ProductWithFilterationForCountSpecifications(specParams);

            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);   // for count pagination
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return brands;
        }

     

        public async Task<IReadOnlyList<ProductCategory>> GetProductCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return categories;
        }

        
    }
}
