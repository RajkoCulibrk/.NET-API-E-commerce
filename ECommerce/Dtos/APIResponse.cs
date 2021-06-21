using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos
{
    public class APIResponse<T>
    {
        public bool Success { get; set; } = true;
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
       
    }
}
