using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Products
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public double Price { get; set; }
        public bool New { get; set; }
        public bool Featured { get; set; }
        public int CategoryId { get; set; }
        public IFormFile file { get; set; }
    }
}
