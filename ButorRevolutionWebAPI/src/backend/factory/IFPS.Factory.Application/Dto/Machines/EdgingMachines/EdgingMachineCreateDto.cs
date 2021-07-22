using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class EdgingMachineCreateDto : MachineCreateDto
    {
        public EdgingMachine CreateModelObject()
        {
            return new EdgingMachine(name: MachineName, brandId: BrandId)
            {
                SoftwareVersion = SoftwareVersion,
                SerialNumber = SerialNumber,
                Code = Code,
                YearOfManufacture = YearOfManufacture
            };
        }
    }
}
