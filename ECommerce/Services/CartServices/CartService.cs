using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Cart;
using ECommerce.Dtos.Orders;
using ECommerce.Dtos.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private IMapper _mapper;

        public CartService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<CartItemDto>> AddToCart(AddToCartDto addToCartDto, string userId)
        {
            APIResponse<CartItemDto> response = new APIResponse<CartItemDto>();
            var exists = await _context.Cart
                .Include(ci=>ci.Product)
                .FirstOrDefaultAsync(ci => ci.ProductId == addToCartDto.ProductId && ci.UserId == userId);
            if (exists != null)
            {
                if (addToCartDto.Ammount == 0)
                {
                    _context.Cart.Remove(exists);
                    exists.Ammount = 0;
                    response.Data = _mapper.Map<CartItemDto>(exists);
                    response.Message = "Item removed from cart";
                    response.StatusCode = 200;
                    response.Success = true;

                }
                else
                {
                    exists.Ammount = addToCartDto.Ammount;
                    _context.Update(exists);
                    response.Data = _mapper.Map<CartItemDto>(exists);
                    response.Message = "Quantity updated";
                    response.StatusCode = 200;
                    response.Success = true;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                if (addToCartDto.Ammount == 0)
                {
                    response.Message = "Cart item not found";
                    response.StatusCode=404;
                    response.Success = false;
                   
                    return response;
                }
                var cartItem = new CartItem
                {
                    ProductId = addToCartDto.ProductId,
                    UserId = userId,
                    Ammount= addToCartDto.Ammount
                };
                await _context.Cart.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                var newCartItem =await _context.Cart
                    .Include(ci=>ci.Product)
                    .FirstOrDefaultAsync(ci=>ci.ProductId== addToCartDto.ProductId && ci.UserId==userId);
                response.Data = _mapper.Map<CartItemDto>(newCartItem);
                response.Message = "Item added to the cart";
                response.Success = true;
                response.StatusCode = 200;

            }
            return response;
        }

        public async Task<APIResponse<List<CartItemDto>>> GetCartItems(string userId)
        {
            APIResponse<List<CartItemDto>> response = new APIResponse<List<CartItemDto>>();
            var result = await _context.Cart.
                Include(ci => ci.Product)
                .ThenInclude(p=>p.Category)
                .Where(ci => ci.UserId == userId)
                .Select(x => _mapper.Map<CartItemDto>(x)).ToListAsync();
            response.Data = result;


            return response;
        }
    }
}
