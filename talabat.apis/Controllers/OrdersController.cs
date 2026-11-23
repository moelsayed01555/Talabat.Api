using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using talabat.apis.DTOs;
using talabat.apis.Errors;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace talabat.apis.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder( OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);  // get the email of the user that is logged in

            // map addressDto to address to be able to send it to orderService to create order
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail , orderDto.BasketId , orderDto.DeliveryMethodId ,address);

            if(order == null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }

            return Ok(_mapper.Map<Talabat.Core.Entites.Order_Aggregate.Order, OrderToReturnDto>(order));
        }



        // بنجيب الاوردرات بتاع يوسر معين عن طريق الايميل بتاعه

        [HttpGet] // GET api/orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(/*string email*/)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);  // get the email of the user that is logged in
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Talabat.Core.Entites.Order_Aggregate.Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }



        // بنجيب اوردر معين عن طريق الاوردر ايدي والايميل بتاع اليوزر
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")] // GET api/orders/1
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id /*string email*/)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);  // get the email of the user that is logged in
            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            if (order == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(_mapper.Map<Talabat.Core.Entites.Order_Aggregate.Order, OrderToReturnDto>(order));
        }


        // بجيب كل الدليفري ميثودز اللي موجودة في الداتا بيز
        [HttpGet("deliveryMethods")] // GET api/orders/deliveryMethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}
