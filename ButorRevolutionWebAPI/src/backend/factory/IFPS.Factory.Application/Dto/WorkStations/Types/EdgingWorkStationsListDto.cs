using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class EdgingWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int SecPerEdge { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public EdgingWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            SecPerEdge = workStation.SecPerComponent;
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
