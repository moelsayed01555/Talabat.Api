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


    }
}
