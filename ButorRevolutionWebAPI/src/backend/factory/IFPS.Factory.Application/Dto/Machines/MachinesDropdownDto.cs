using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MachinesDropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MachinesDropdownDto(Machine machine)
        {
            Id = machine.Id;
            Name = machine.Name;
        }
    }
}
