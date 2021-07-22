using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class OrderFinanceFilterDto : OrderedPagedRequestDto
    {
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public int? CurrentStatusId { get; set; }
        public DateTime? StatusDeadlineFrom { get; set; }
        public DateTime? StatusDeadlineTo { get; set; }
        public DateTime? DeadlineFrom { get; set; }
        public DateTime? DeadlineTo { get; set; }
        public int Price { get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { nameof(OrderName), nameof(Order.OrderName) },
                { nameof(WorkingNumber), nameof(Order.WorkingNumber) },
                { nameof(CurrentStatusId), nameof(Order.CurrentTicket.OrderStateId) },
            };
        }        
    }

}
