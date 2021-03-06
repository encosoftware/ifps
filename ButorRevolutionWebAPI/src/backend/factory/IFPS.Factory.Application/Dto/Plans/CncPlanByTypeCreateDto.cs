using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CncPlanByTypeCreateDto : BasePlanByTypeCreateDto
    {
        public CncPlanByTypeCreateDto() : base()
        {

        }

        public CncPlan CreateModelObject()
        {
            var date = DateTime.Now;

            return new CncPlan()
            {
                ScheduledStartTime = new DateTime(date.Year, date.Month, date.Day, ScheduledStartHour, ScheduledStartMinute, 0),
                ScheduledEndTime = new DateTime(date.Year, date.Month, date.Day, ScheduledEndHour, ScheduledEndMinute, 0),
                ConcreteFurnitureComponentId = ConcreteFurnitureComponentId,
                ConcreteFurnitureUnitId = ConcreteFurnitureUnitId,
                WorkStationId = WorkStationId
            };
        }
    }
}
