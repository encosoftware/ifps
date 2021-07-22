using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class StockStatisticsDetailsDto
    {
        public string MaterialCode { get; set; }
        public List<StockStatisticsQuantityDto> Quantities { get; set; }

        public StockStatisticsDetailsDto(string materialCode)
        {
            MaterialCode = materialCode;
            Quantities = new List<StockStatisticsQuantityDto>();
        }
    }
}