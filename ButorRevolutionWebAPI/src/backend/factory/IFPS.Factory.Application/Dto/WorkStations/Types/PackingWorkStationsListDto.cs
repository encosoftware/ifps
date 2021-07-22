using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class PackingWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int PackingSecPerComponent { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public PackingWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            PackingSecPerComponent = workStation.SecPerComponent;
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
