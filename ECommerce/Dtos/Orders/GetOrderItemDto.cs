using ECommerce.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Orders
{
    public class GetOrderItemDto
    {
        public GetProductDto Product { get; set; }
        public int Ammount { get; set; }
        public double Price { get; set; }

        public double Total()
        {
            return Ammount * Price;
        }
    }
}
