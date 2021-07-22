using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class SalesPersonStatisticsDto
    {
        public SalesPersonSummaryDto Summary { get; set; }
        public List<SalesPersonListDto> SalesPersons { get; set; }

        public SalesPersonStatisticsDto()
        {
            Summary = new SalesPersonSummaryDto();
            SalesPersons = new List<SalesPersonListDto>();
        }
    }
}
