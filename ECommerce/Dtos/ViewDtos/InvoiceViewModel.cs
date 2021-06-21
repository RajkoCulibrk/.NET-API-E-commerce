using ECommerce.Data.Models;
using ECommerce.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.ViewDtos
{
    public class InvoiceViewModel
    {
        public GetOrderDto Order { get; set; }
        public List<GetOrderItemDto> OrderItems { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }

        public double Total()
        {
           return OrderItems.Sum(oi=>oi.Total());
        }
    }
}
