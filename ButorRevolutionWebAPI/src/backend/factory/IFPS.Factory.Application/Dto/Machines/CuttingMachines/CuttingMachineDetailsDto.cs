using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingMachineDetailsDto : MachineDetailsDto
    {
        public CuttingMachineDetailsDto(CuttingMachine machine) : base(machine)
        {

        }
    }
}
