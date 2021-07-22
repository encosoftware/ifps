using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class EdgingMachineDetailsDto : MachineDetailsDto
    {
        public EdgingMachineDetailsDto(EdgingMachine machine) : base(machine) { }
    }
}
