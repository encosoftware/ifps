using System;

namespace IFPS.Sales.Application.Dto
{
    public class VenueDateRangeCreateDto
    {
        public int DayTypeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public VenueDateRangeCreateDto(VenueDateRangeCreateDto companyDateRange)
        {
            DayTypeId = companyDateRange.DayTypeId;
            From = companyDateRange.From;
            To = companyDateRange.To;
        }
    }
}
