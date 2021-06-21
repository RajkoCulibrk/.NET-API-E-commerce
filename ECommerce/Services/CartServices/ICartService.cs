using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Cart;
using ECommerce.Dtos.Orders;
using ECommerce.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.CartServices
{
    public interface ICartService
    {
        Task<APIResponse<CartItemDto>> AddToCart(AddToCartDto addToCartDto, string userId);
        Task<APIResponse<List<CartItemDto>>> GetCartItems(string userId);
       
       
    }
}
