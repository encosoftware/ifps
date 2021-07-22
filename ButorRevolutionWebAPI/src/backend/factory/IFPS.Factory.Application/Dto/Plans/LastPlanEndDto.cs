using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class LastPlanEndDto
    {
        public string Date { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public LastPlanEndDto(Plan plan)
        {
            Date = plan.ScheduledEndTime.ToString("yyyy-MM-dd");
            Hour = plan.ScheduledEndTime.Hour;
            Minute = plan.ScheduledEndTime.Minute;
        }
    }
}
