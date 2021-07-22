using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class SortingFilterDto : OrderedPagedRequestDto
    {
        public string UnitName { get; set; }
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string WorkerName { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(OrderName), nameof(ProductionProcess.Order.OrderName) },
                { nameof(WorkingNumber), nameof(ProductionProcess.Order.WorkingNumber) }
            };
        }
    }
}
