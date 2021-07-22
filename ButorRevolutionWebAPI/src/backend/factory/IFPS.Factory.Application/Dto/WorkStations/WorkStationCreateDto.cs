using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationCreateDto
    {
        public string Name { get; set; }
        public int OptimalCrew { get; set; }
        public bool IsActive { get; set; }
        public int MachineId { get; set; }
        public int WorkStationTypeId { get; set; }

        public WorkStationCreateDto() { }

        public WorkStation CreateModelObject()
        {
            return new WorkStation(Name, OptimalCrew, IsActive, WorkStationTypeId) { MachineId = MachineId };
        }
    }
}
