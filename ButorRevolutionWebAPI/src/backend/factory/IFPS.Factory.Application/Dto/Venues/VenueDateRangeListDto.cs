using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class VenueDateRangeListDto
    {
        public DayTypeListDto DayType { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public VenueDateRangeListDto(VenueDateRange venueDateRange)
        {
            DayType = new DayTypeListDto(venueDateRange.DayType);
            From = venueDateRange.Interval.From;
            To = venueDateRange.Interval.To;
        }
    }
}