using API.Exceptions;
using AutoMapper;
using Core.Dtos.Orders;
using Core.Entities.Order;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public OrdersController(IOrderService orderService, IMapper mapper, IMailService mailService)
        {

            _mapper = mapper;
            _orderService = orderService;
            _mailService = mailService;

        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {

            var email = User.Claims.ToList()[0].Value;

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null) return BadRequest(new APIResponse(400, "Problem creating order"));


            var message = new MailRequest { ToEmail = email, Subject = "Order Created", Body = "Your order has been created" };
            await _mailService.SendEmailAsync(message);
          
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = User.Claims.ToList()[0].Value;

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = User.Claims.ToList()[0].Value;

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new APIResponse(404));

            return _mapper.Map<OrderToReturnDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}
