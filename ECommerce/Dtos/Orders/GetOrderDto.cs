using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Orders
{
    public class GetOrderDto
    {
        public int OrderId { get; set; }

        public bool Confirmed { get; set; }
        public bool Sent { get; set; }

        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HouseNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
