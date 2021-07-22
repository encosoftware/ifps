using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class OrdersForOptimizationDto
    {
        public List<Guid> OrderIds { get; set; }
        public int ShiftNumber { get; set; }
        public int ShiftLengthHours { get; set; }
        public int SortingStrategyTypeId { get; set; }
    }
}
