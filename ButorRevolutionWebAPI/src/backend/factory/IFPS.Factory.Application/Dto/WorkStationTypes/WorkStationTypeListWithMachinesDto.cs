using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class WorkStationTypeListWithMachinesDto
    {
        public int Id { get; set; }

        public WorkStationTypeEnum Type { get; set; }

        public string Translation { get; set; }

        public List<MachinesDropdownDto> Machines { get; set; }

        public WorkStationTypeListWithMachinesDto(WorkStationType type, List<Machine> machines)
        {
            Id = type.Id;
            Type = type.StationType;
            Translation = type.CurrentTranslation?.Name;
            Machines = new List<MachinesDropdownDto>();
            foreach(var machine in machines)
            {
                Machines.Add(new MachinesDropdownDto(machine));
            }
        }
    }
}
