using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using talabat.apis.Helpres;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace talabat.apis.Controllers
{
    public class ProductsController : BaseApiController
    {
        ///private readonly IGenericRepository<Product> _productsRpo;
        ///private readonly IGenericRepository<ProductBrand> _brandsRpo;
        ///private readonly IGenericRepository<ProductCategory> _categoriesRpo;
        
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        #region Before applying unit of work
        //public ProductsController( IGenericRepository<Product> productsRpo ,IGenericRepository<ProductBrand> brandsRpo ,IGenericRepository<ProductCategory> categoriesRpo ,IMapper mapper)
        //{
        //    _productsRpo = productsRpo;
        //    _brandsRpo = brandsRpo;
        //    _categoriesRpo = categoriesRpo;
        //    _mapper = mapper;
        //}
        #endregion
        public ProductsController( IProductService productService , IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        //Get : api/Products  // [FromQuery] means read query from url header
        [Authorize/*(AuthenticationSchemes = "Bearer")*/]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProsuctSpecsParams specParams)     // criteria (null) => all products
        {
            var products = await _productService.GetProductsAsync(specParams);

            var count = await _productService.GetCountAsync(specParams);   // for count pagination

            var productDtoData = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            return Ok(new Pagination<ProductDto>(specParams.PageIndex , specParams.PageSize , count , productDtoData));
        }


        // api/GetProduct/id
        [ProducesResponseType(typeof(ProductDto) , statusCode: 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), statusCode: 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductId(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiErrorResponse(404)); 
            }

            var productDto = _mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }


        // api/Products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetProductBrandsAsync();

            return Ok(brands);
        }

        // api/Products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetProductCategoriesAsync();

            return Ok(categories);
        }

    }
}
