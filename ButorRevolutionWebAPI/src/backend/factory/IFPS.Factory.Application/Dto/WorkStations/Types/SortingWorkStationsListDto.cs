using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class SortingWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int SortingSecPerComponent { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public SortingWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            SortingSecPerComponent = workStation.SecPerComponent;
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
