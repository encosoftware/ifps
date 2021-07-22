using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class OrderSchedulingFilterDto : OrderedPagedRequestDto
    {
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public int? OrderStatusId { get; set; }
        public double Completion { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(OrderName), nameof(Order.OrderName) },
                { nameof(WorkingNumber), nameof(Order.WorkingNumber) },
                { nameof(OrderStatusId), nameof(Order.CurrentTicket.OrderStateId) }
            };
        }
    }
}
