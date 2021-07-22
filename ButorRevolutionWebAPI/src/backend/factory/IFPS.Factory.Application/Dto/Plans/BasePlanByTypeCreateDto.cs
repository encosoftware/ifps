using System;

namespace IFPS.Factory.Application.Dto
{
    public class BasePlanByTypeCreateDto
    {
        public int ScheduledStartHour { get; set; }
        public int ScheduledEndHour { get; set; }
        public int ScheduledStartMinute { get; set; }
        public int ScheduledEndMinute { get; set; }
        public int? ConcreteFurnitureComponentId { get; set; }
        public int? ConcreteFurnitureUnitId { get; set; }
        public int WorkStationId { get; set; }
        public Guid? OrderId { get; set; }

        public BasePlanByTypeCreateDto()
        {

        }
    }
}
