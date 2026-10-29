using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace talabat.apis.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        // allow DI
        public BasketController(IBasketRepository basketRepo , IMapper mapper)
        {
            
           _basketRepo = basketRepo;
            _mapper = mapper;
        }

        // First Endpoint to get Basket
        // Get: baseurl/api/basket?id=...
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket( string id )     // retrun actionresult from type CustomerBasket
        {
            var basket = await _basketRepo.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id)); 
        }


        // Second EndPoint to create or update basket
        // Post : baseurl/api/basket

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket( CustomerBasketDto customerBasketDto)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasketDto);

            var createdOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(mappedBasket);

            if(createdOrUpdatedBasket is null)
            { 
                return BadRequest(new ApiErrorResponse(400));         // we use apiErrorResponse beacuse we made it to handle error
            }

            return Ok(createdOrUpdatedBasket);
        }


        // Endpoint to delete basket
        // Delete: baseurl/api/basket?id=...
        [HttpDelete]
        public async Task DeleteBasket(string id)                      // it will return nothing so we made it Task only
        {
            await _basketRepo.DeleteBasketAsync(id);
        }
    }
}
