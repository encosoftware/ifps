using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class PlanByTypeCreateDto : BasePlanByTypeCreateDto
    {
        

        public PlanByTypeCreateDto() : base()
        {

        }

        public Plan CreateModelObject()
        {
            var date = DateTime.Now;

            return new Plan()
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
