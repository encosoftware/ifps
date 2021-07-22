using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CNCMachinesDropdownDto : MachinesDropdownDto
    {
        public CNCMachinesDropdownDto(CncMachine machine): base(machine)
        {

        }
    }
}
