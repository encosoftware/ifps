using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingMachineUpdateDto : MachineUpdateDto
    {
        public CuttingMachine UpdateModelObject(CuttingMachine machine)
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
