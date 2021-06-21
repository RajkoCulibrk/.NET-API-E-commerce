using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.ViewDtos
{
    public class ConfirmSentModel
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
    }
}
