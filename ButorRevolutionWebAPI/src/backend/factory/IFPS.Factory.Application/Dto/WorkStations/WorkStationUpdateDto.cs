using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationUpdateDto
    {
        public string Name { get; set; }
        public int OptimalCrew { get; set; }
        public int MachineId { get; set; }
        public int WorkStationTypeId { get; set; }

        public WorkStationUpdateDto() { }

        public WorkStation UpdateModelObject(WorkStation workStation)
        {
            if(!workStation.Name.Equals(Name))
            {
                workStation.Name = Name;
            }

            if(workStation.OptimalCrew != OptimalCrew)
            {
                workStation.OptimalCrew = OptimalCrew;
            }

            if(workStation.MachineId != MachineId)
            {
                workStation.MachineId = MachineId;
            }

            if(workStation.WorkStationTypeId != WorkStationTypeId)
            {
                workStation.WorkStationTypeId = WorkStationTypeId;
            }

            return workStation;
        }
    }
}
