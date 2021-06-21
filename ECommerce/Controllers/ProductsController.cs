using ECommerce.Dtos;
using ECommerce.Dtos.Products;
using ECommerce.Helpers;
using ECommerce.Services.EMailService;
using ECommerce.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;
        public ProductsController(IProductService productService, IEmailService emailService)
        {
            _productService = productService;
            _emailService = emailService;
        }

        
        [HttpGet]
   
      
        public async Task<IActionResult> GetAll([FromQuery] RequestParams requestParams)
        {
           var products= await _productService.GetAll(requestParams);
           
            return Ok(products);
        }

        [HttpGet]
        [Route("new")]
        public async Task<IActionResult> GetNew()
        {
            var response =await _productService.GetNew();
            return Ok(response);
        }

        [HttpGet]
        [Route("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            var response = await _productService.GetFeatured();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await _productService.GetSingleProduct(id);
            if (response.Code == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productData)
        {
          var product=  await _productService.CreateProduct(productData);
            return Ok(product);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response=await _productService.DeleteProduct(id);

            return StatusCode(response.Code,response);
        }

        [Authorize(Roles =("Administrator"))]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var response = await _productService.UpdateProduct(productDto);
            if (response.Code == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = ("Administrator"))]
        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> UploadPicture(int id, IFormFile file)
        {
            var response = await _productService.AddProductImage(id, file);
            if (response.Code == 404)
            {
                return NotFound(response);
            }
            return Ok(response);

        }
        [Authorize(Roles = ("Administrator"))]
        [HttpDelete]
        [Route("images/{id:int}")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var response = await _productService.DeleteProductImage(id);
            if (response.Code == 404)
            {
                return NotFound(response);
            }
            return Ok(response);

        }
    }
}
