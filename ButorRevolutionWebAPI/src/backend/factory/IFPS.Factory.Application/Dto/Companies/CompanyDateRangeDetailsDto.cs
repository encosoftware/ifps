using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CompanyDateRangeDetailsDto
    {
        public int DayTypeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public CompanyDateRangeDetailsDto(CompanyDateRange companyDateRange)
        {
            DayTypeId = companyDateRange.DayTypeId;
            From = companyDateRange.Interval.From;
            To = companyDateRange.Interval.To;
        }
    }
}