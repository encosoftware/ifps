using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationsPlansDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? EndTime { get; set; }

        public WorkStationsPlansDetailsDto(WorkStation workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            EndTime = SetEndTime(workStation);
        }

        private DateTime SetEndTime(WorkStation workStation)
        {
            var dates = new List<DateTime>();
            foreach (var plan in workStation.Plans)
            {
                dates.Add(plan.ScheduledEndTime);
            }
            return dates.Count() != 0 ? dates.Max() : new DateTime();
        }
    }
}
