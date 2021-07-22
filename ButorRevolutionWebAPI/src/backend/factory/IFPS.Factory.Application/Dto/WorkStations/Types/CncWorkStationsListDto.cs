using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class CncWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int DistancePerSec { get; set; }
        public int SecPerHole { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public CncWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            DistancePerSec = workStation.SecPerComponent;
            SecPerHole = 10; // TODO
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
