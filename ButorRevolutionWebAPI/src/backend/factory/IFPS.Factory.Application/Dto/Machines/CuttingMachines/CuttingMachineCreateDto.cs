using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingMachineCreateDto : MachineCreateDto
    {
        public CuttingMachine CreateModelObject()
        {
            return new CuttingMachine(name: MachineName, brandId: BrandId)
            {
                SoftwareVersion = SoftwareVersion,
                SerialNumber = SerialNumber,
                Code = Code,
                YearOfManufacture = YearOfManufacture
            };
        }
    }
}
