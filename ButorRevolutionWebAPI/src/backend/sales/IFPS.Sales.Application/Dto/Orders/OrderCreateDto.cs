using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderCreateDto
    {
        public int CustomerUserId { get; set; }
        public string OrderName { get; set; }
        public AddressCreateDto ShippingAddress { get; set; }
        public int SalesPersonUserId { get; set; }
        public DateTime Deadline { get; set; }

        public OrderCreateDto()
        { }
    }
}
