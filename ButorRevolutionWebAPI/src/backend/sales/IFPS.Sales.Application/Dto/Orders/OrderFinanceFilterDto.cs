using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class OrderFinanceFilterDto : OrderedPagedRequestDto
    {
        public string OrderId { get; set; }
        public string WorkingNumber { get; set; }
        public int? CurrentStatusId { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public string Customer { get; set; }
        public DateTime? DeadlineFrom { get; set; }
        public DateTime? DeadlineTo { get; set; }
        public int Price { get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { nameof(OrderId), nameof(Order.OrderName) },
                { nameof(WorkingNumber), nameof(Order.WorkingNumber) },
                { nameof(CurrentStatusId), nameof(Order.CurrentTicket.OrderStateId) },
                { nameof(StatusDeadline), nameof(Order.CurrentTicket.Deadline) },
                { nameof(Customer), nameof(Order.Customer.User.CurrentVersion.Name)},
            };
        }
    }
}
