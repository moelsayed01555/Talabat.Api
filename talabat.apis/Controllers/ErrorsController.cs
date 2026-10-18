using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.apis.Errors;

namespace talabat.apis.Controllers
{
    [Route("errors/{code}")]     // code for any endpoint that not found (404) or any other error code
    [ApiController]
    public class ErrorsController : ControllerBase
    {

        // If the client requests an endpoint that does not exist , a 404 error code to the client

        [HttpGet]
        public ActionResult Error(int code)
        {
            return NotFound(new ApiErrorResponse(code));  // return the error code to the client
        }
    }
}
