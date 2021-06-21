using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string PublicUrl { get; set; }
        public string CloudinaryId { get; set; }
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool New { get; set; }
        public bool Featured { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductImage> Images { get; set; }
    }
}
