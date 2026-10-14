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
        private readonly IMapper _mapper;

        public ProductsController( IGenericRepository<Product> productsRpo , IMapper mapper)
        {
            _productsRpo = productsRpo;
            _mapper = mapper;
        }

        // api/GetProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()     // criteria (null) => all products
        {
            var spec = new ProductWithBrandAndCategorySpecifications();
            var products = await _productsRpo.GetAllSpecAsync(spec);

            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }


        // api/GetProduct/id
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



    }
}
