using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos
{
    public class PaginationResponse<T>
    {
        public T Data { get; set; }
        public int Pages { get; set; }
        public int Total { get; set; }
    }
}
