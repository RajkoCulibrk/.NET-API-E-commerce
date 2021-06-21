using ECommerce.Dtos.Cart;
using ECommerce.Helpers;
using ECommerce.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDto addToCartDto)
        {
            var userId = User.GetUserId();
           var response= await _cartService.AddToCart(addToCartDto, userId);
            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
           var userId = User.GetUserId();
           var result= await _cartService.GetCartItems(userId);
            return Ok(result);

        }
    }
}
