using System;

namespace IFPS.Factory.Application.Dto
{
    public class StockStatisticsFilterDto
    {
        public Guid MaterialId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}