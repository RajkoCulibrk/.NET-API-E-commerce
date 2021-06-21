using ECommerce.Data.Models;
using ECommerce.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Cart
{
    public class CartItemDto
    {
        public GetProductDto Product { get; set; }
        public int Ammount { get; set; }
    }
}
