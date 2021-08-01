using AutoMapper;
using Core.Dtos.Products;
using Core.Entities.Product;
using Core.Interfaces;
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
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Repository<Product>().ListAllAsync();
            return Ok(products);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreationDto product)
        {
            var productToCreate = _mapper.Map<Product>(product);
            _unitOfWork.Repository<Product>().Add(productToCreate);
            await _unitOfWork.Complete();
          
            return Ok();
        }
    }
}
