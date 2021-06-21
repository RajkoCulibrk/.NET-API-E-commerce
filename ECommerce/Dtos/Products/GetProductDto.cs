using ECommerce.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Products
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublicUrl { get; set; }
        public double Price { get; set; }
        public bool New { get; set; }
        public string ShortDescription { get; set; }
        public bool Featured { get; set; }
        public GetCategoryDto Category { get; set; }
        public List<GetProductImageDto> Images { get; set; }
    }
}
