using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MachineWorkstationListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MachineWorkstationListDto(Machine machine)
        {
            Id = machine.Id;
            Name = machine.Name;
        }
    }
}
