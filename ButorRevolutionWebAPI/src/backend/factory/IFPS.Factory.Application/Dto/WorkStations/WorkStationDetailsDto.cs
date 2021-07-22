using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationDetailsDto
    {
        public string Name { get; set; }
        public int OptimalCrew { get; set; }
        public bool IsActive { get; set; }
        public int? MachineId { get; set; }
        public int WorkStationTypeId { get; set; }

        public WorkStationDetailsDto(WorkStation workStation)
        {
            Name = workStation.Name;
            OptimalCrew = workStation.OptimalCrew;
            IsActive = workStation.IsActive;
            MachineId = workStation.MachineId;
            WorkStationTypeId = workStation.WorkStationTypeId;
        }
    }
}
