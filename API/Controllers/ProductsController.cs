using API.Helpers;
using AutoMapper;
using Core.Dtos.Products;
using Core.Entities.Product;
using Core.Interfaces;
using Core.Specifications;
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
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParam productParam)
        {

            var productWithSpec = new ProductWithTypeBrandPictureSpecification(productParam);

            var productsList = await _unitOfWork.Repository<Product>().ListAsync(productWithSpec);
            var productDto = _mapper.Map<IReadOnlyList<ProductDto>>(productsList);

            var productCount = await _unitOfWork.Repository<Product>().CountAsync(productWithSpec);


            var paginatedResult = new Pagination<ProductDto>(productParam.PageIndex, productParam.PageSize, productCount, productDto);

            return Ok(paginatedResult);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreationDto product)
        {
            var productToCreate = _mapper.Map<Product>(product);
            _unitOfWork.Repository<Product>().Add(productToCreate);
            await _unitOfWork.Complete();
          
            return Ok();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().ListAllAsync());
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().ListAllAsync());
        }
    }
}
