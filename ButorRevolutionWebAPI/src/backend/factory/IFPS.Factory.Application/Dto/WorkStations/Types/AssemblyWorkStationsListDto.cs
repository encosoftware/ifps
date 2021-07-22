using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class AssemblyWorkStationsListDto : BaseWorkStationTypeListDto
    {
        public int AssemblySecPerComponent { get; set; }
        public int SecPerAccessory { get; set; }
        public LastPlanEndDto LastPlanEnd { get; set; }

        public AssemblyWorkStationsListDto(WorkStation workStation) : base(workStation)
        {
            Id = workStation.Id;
            Name = workStation.Name;
            Type = workStation.WorkStationTypeId;
            AssemblySecPerComponent = workStation.SecPerComponent;
            SecPerAccessory = 2; // TODO
            LastPlanEnd = new LastPlanEndDto(GetLastPlan(workStation));
        }
    }
}
