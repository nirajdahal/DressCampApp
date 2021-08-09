using AutoMapper;
using Core.Dtos.Basket;
using Core.Entities.Basket;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUserBasketService _userBasket;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper, IUserBasketService userBasket)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _userBasket = userBasket;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {

            var email = User.Claims.ToList()[0].Value;
            var basketId = await _userBasket.GetBasketIdForUser(email);
          
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return Ok(basket ?? new CustomerBasket(basketId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            
            var email = User.Claims.ToList()[0].Value;
            var basketId = await _userBasket.GetBasketIdForUser(email);
            basket.Id = basketId;
            var basketToUpdate = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basketToUpdate);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            var email = User.Claims.ToList()[0].Value;
            await _userBasket.RemoveBasketIdFromUser(email);
        }
    }
}
