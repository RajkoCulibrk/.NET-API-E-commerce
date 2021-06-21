using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Cart
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Ammount { get; set; }
    }
}
