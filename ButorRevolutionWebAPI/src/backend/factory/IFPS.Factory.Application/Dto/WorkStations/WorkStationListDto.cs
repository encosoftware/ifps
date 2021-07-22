using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OptimalCrew { get; set; }
        public MachineWorkstationListDto MachineName { get; set; }
        public WorkStationTypeListDto WorkStationType { get; set; }
        public bool Status { get; set; }

        public WorkStationListDto() { }

        public static Func<WorkStation, WorkStationListDto> FromEntity = entity => new WorkStationListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            OptimalCrew = entity.OptimalCrew,
            MachineName = new MachineWorkstationListDto(entity.Machine),
            WorkStationType = new WorkStationTypeListDto(entity.WorkStationType),
            Status = entity.IsActive
        };
    }
}
