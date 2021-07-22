using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class ManualLabourPlanByTypeCreateDto : BasePlanByTypeCreateDto
    {
        public ManualLabourPlanByTypeCreateDto() : base()
        {

        }

        public ManualLaborPlan CreateModelObject()
        {
            var date = DateTime.Now;

            return new ManualLaborPlan()
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
