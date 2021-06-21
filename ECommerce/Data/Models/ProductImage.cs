using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public Product Product { get; set; }
        public string PublicUrl { get; set; }
        public string CloudinaryId { get; set; }
    }
}
