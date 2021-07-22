using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CncMachineDetailsDto : MachineDetailsDto
    {
        public string SharedFolderPath { get; set; }
        public CncMachineDetailsDto(CncMachine machine) : base(machine)
        {
            SharedFolderPath = machine.SharedFolderPath;
        }
    }
}
