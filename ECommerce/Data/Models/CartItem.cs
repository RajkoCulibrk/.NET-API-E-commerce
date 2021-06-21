using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Models
{
    public class CartItem
    {
        
        public string UserId { get; set; }
        public ApiUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int Ammount { get; set; } = 1;
    }
}
