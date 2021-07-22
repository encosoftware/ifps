using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WorkingHourDto
    {
        public int? Id { get; set; }
        public int DayTypeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public static WorkingHourDto FromModel(SalesPersonDateRange ent)
        {
            return new WorkingHourDto
            {
                Id = ent.Id,
                DayTypeId = ent.DayTypeId,
                From = ent.Interval.From,
                To = ent.Interval.To,
            };
        }
    }
}
