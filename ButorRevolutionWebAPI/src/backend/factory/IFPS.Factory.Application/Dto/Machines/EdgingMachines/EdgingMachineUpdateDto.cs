using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class EdgingMachineUpdateDto : MachineUpdateDto
    {
        public EdgingMachine UpdateModelObject(EdgingMachine machine)
        {
            machine.Name = MachineName;
            machine.SoftwareVersion = SoftwareVersion;
            machine.SerialNumber = SerialNumber;
            machine.Code = Code;
            machine.YearOfManufacture = YearOfManufacture;
            machine.BrandId = BrandId;
            return machine;
        }
    }
}
