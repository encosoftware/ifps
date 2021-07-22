using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class LayoutWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int CuttingSecPerComponent { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public LayoutWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            CuttingSecPerComponent = workStation.SecPerComponent;
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
