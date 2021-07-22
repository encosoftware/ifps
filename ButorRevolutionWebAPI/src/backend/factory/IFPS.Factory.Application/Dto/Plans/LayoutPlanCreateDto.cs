using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class LayoutPlanCreateDto : BasePlanByTypeCreateDto
    {
        public Guid BoardId { get; set; }
        public List<Guid> OrderIds { get; set; }
        public List<CuttingCreateDto> Cuttings { get; set; }

        public LayoutPlanCreateDto() : base()
        {

        }

        public LayoutPlan CreateModelObject()
        {
            var date = DateTime.Now;

            return new LayoutPlan(BoardId)
            {
                ScheduledStartTime = new DateTime(date.Year, date.Month, date.Day, ScheduledStartHour, ScheduledStartMinute, 0),
                ScheduledEndTime = new DateTime(date.Year, date.Month, date.Day, ScheduledEndHour, ScheduledEndMinute, 0),
                WorkStationId = WorkStationId
            };
        }
    }
}
