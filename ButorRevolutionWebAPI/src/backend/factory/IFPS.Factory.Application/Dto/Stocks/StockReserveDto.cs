using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class StockReserveDto
    {
        public List<StockQuantityDto> StockQuantities { get; set; }
        public Guid OrderId { get; set; }
    }
}
