using ENCO.DDD.Application.Dto;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class OptimizationFilterDto : OrderedPagedRequestDto
    {
        public int? ShiftNumber { get; set; }
        public int? ShiftLength { get; set; }
        public DateTime? StartedAtFrom { get; set; }
        public DateTime? StartedAtTo { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
            };
        }
    }
}
