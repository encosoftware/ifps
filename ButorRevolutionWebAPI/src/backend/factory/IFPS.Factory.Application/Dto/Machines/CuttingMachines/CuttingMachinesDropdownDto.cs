using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingMachinesDropdownDto : MachinesDropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CuttingMachinesDropdownDto(CuttingMachine machine) : base(machine)
        {

        }
    }
}
