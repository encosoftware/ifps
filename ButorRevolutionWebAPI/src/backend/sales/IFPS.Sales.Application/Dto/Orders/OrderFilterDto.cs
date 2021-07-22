using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class OrderFilterDto : OrderedPagedRequestDto
    {
        public string OrderId { get; set; }
        public string WorkingNumber { get; set; }
        public int? CurrentStatusId { get; set; }
        public DateTime? StatusDeadlineFrom { get; set; }
        public DateTime? StatusDeadlineTo { get; set; }
        public string Responsible { get; set; }
        public string Customer { get; set; }
        public string Sales { get; set; }
        public DateTime? CreatedOnFrom { get; set; }
        public DateTime? CreatedOnTo { get; set; }
        public DateTime? DeadlineFrom { get; set; }
        public DateTime? DeadlineTo{ get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            var columnMappings = new Dictionary<string, string>
            {
                { nameof(OrderId), nameof(Order.OrderName) },
                { nameof(WorkingNumber), nameof(Order.WorkingNumber) },
                { nameof(CurrentStatusId), nameof(Order.CurrentTicket.OrderStateId) },
                { nameof(Responsible), nameof(Order.CurrentTicket.AssignedTo.CurrentVersion.Name)},
                { nameof(Customer), nameof(Order.Customer.User.CurrentVersion.Name)},
                { nameof(Sales), nameof(Order.SalesPerson.User.CurrentVersion.Name)},
            };

            return columnMappings;
        }
    }
}
