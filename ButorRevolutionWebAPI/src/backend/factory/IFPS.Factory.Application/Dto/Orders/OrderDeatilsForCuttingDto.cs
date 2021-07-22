using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderDeatilsForCuttingDto
    {
        public Guid Guid { get; set; }

        public string OrderName { get; set; }

        public DateTime Deadline { get; set; }

        public OrderDeatilsForCuttingDto(Order order)
        {
            Guid = order.Id;
            OrderName = order.OrderName;
            Deadline = order.Deadline;
        }
    }
}
