using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Orders
{
    public class OrderItemInvoiceDto
    {
      
            public int Ammount { get; set; }
            public double Price { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
    }
}
