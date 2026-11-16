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
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder( OrderDto orderDto)
        {
            // map addressDto to address to be able to send it to orderService to create order
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail , orderDto.BasketId , orderDto.DeliveryMethodId ,address);

            if(order == null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }

            return Ok(_mapper.Map<Talabat.Core.Entites.Order_Aggregate.Order, OrderToReturnDto>(order));
        }



        // بنجيب الاوردرات بتاع يوسر معين عن طريق الايميل بتاعه

        [HttpGet] // GET api/orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(string email)
        {
            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Talabat.Core.Entites.Order_Aggregate.Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }



        // بنجيب اوردر معين عن طريق الاوردر ايدي والايميل بتاع اليوزر
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")] // GET api/orders/1
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id, string email)
        {
            var order = await _orderService.GetOrderByIdForUserAync(id, email);
            if (order == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(_mapper.Map<Talabat.Core.Entites.Order_Aggregate.Order, OrderToReturnDto>(order));
        }

    }
}
