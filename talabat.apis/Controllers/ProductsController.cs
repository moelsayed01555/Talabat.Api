using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace talabat.apis.Controllers
{
    //[Route("api/[controller]")] مش محتاجينهم خلاص 
    //[ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRpo;

        public ProductsController( IGenericRepository<Product> productsRpo)
        {
            _productsRpo = productsRpo;
        }

        // api/GetProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var result = await _productsRpo.GetAllAsync();

            return Ok(result);
        }


        // api/GetProduct/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductId(int id)
        {
            var result = await _productsRpo.GetAsync(id);

            if(result == null)
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = $"Product with id {id} not found" });

            return Ok(result);
        }



    }
}
