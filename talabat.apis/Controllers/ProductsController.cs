using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace talabat.apis.Controllers
{
    //[Route("api/[controller]")] مش محتاجينهم خلاص 
    //[ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRpo;
        private readonly IGenericRepository<ProductBrand> _brandsRpo;
        private readonly IGenericRepository<ProductCategory> _categoriesRpo;
        private readonly IMapper _mapper;

        public ProductsController( IGenericRepository<Product> productsRpo ,IGenericRepository<ProductBrand> brandsRpo ,IGenericRepository<ProductCategory> categoriesRpo ,IMapper mapper)
        {
            _productsRpo = productsRpo;
            _brandsRpo = brandsRpo;
            _categoriesRpo = categoriesRpo;
            _mapper = mapper;
        }

        // api/GetProducts
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(string? sort , int? brandId , int? categoryId)     // criteria (null) => all products
        {
            var spec = new ProductWithBrandAndCategorySpecifications(sort , brandId , categoryId);
            var products = await _productsRpo.GetAllSpecAsync(spec);

            var productsDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            return Ok(productsDto);
        }


        // api/GetProduct/id
        [ProducesResponseType(typeof(ProductDto) , statusCode: 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), statusCode: 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductId(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productsRpo.GetSpecAsync(spec);

            if (product == null)
            {
                // return NotFound(new ProblemDetails { Title = "Not Found", Detail = $"Product with id {id} not found" });
                return NotFound(new ApiErrorResponse(404));  // instead of paaing anonomous object like above line
            }
            var productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }


        // api/Products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRpo.GetAllAsync();

            return Ok(brands);
        }

        // api/Products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoriesRpo.GetAllAsync();

            return Ok(categories);
        }

    }
}
