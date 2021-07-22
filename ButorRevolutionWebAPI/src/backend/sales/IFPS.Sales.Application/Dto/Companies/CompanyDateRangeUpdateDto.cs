using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class CompanyDateRangeUpdateDto
    {
        public int DayTypeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public CompanyDateRangeUpdateDto()
        {

        }

        public DateRange CreateModelObject()
        {
            return new DateRange(From ,To);
        }

    }
}
