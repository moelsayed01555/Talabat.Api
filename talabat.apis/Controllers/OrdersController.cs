using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace talabat.apis.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost] // api/orders
        public async Task<ActionResult<Talabat.Core.Entites.Order_Aggregate.Order>> CreateOrder( OrderDto orderDto)
        {
            // map addressDto to address to be able to send it to orderService to create order
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail , orderDto.BasketId , orderDto.DeliveryMethodId ,address);

            if(order == null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }

            return Ok(order);
        }



        // بنجيب الاوردرات بتاع يوسر معين عن طريق الايميل بتاعه

        [HttpGet] // GET api/orders
        public async Task<ActionResult<IReadOnlyList<Talabat.Core.Entites.Order_Aggregate.Order>>> GetOrdersForUser(string email)
        {
            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(orders);
        }


    }
}
