using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos
{
    public class RequestParams
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public string Like { get; set; } = "";
        public int? CategoryId { get; set; }
        public string Order { get; set; } 
        public string SortBy { get; set; }

        public int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
