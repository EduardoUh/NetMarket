using AutoMapper;
using Core.Entities.Orders;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    // only the registered users will have access to the methods of this class
    [Authorize]
    public class OrderController(IOrderRepository orderRepository, IMapper mapper) : BaseApiController
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email)) return Unauthorized(new CodeErrorResponse(401));

            var address = _mapper.Map<AddressDto, Address>(orderDto.Address);

            var order = await _orderRepository.AddOrderAsync(email, orderDto.ShippingType, orderDto.ShoppingCartId, address);

            if (order == null) return BadRequest(new CodeErrorResponse(400, "An error occurred while creating the order"));

            return Ok(_mapper.Map<Order, OrderResponseDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrders()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            if (email == null) return Unauthorized(new CodeErrorResponse(401));

            var orders = await _orderRepository.GetOrderByUserEmailAsync(email);

            // return Ok(orders);
            //                    Origin                Destination                     data to be mapped from origin to destination
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderResponseDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            if (email == null) return Unauthorized(new CodeErrorResponse(401));

            var order = await _orderRepository.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new CodeErrorResponse(404, "Order not found"));

            return Ok(_mapper.Map<Order, OrderResponseDto>(order));
        }

        [HttpGet("shippingTypes")]
        public async Task<ActionResult<IReadOnlyList<ShippingType>>> GetShippingTypes()
        {
            return Ok(await _orderRepository.GetShippingTypesAsync());
        }

    }
}
