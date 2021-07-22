using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CncMachineCreateDto : MachineCreateDto
    {
        public string SharedFodlerPath { get; set; }

        public CncMachine CreateModelObject()
        {
            return new CncMachine(name: MachineName, brandId: BrandId)
            {
                SoftwareVersion = SoftwareVersion,
                SerialNumber = SerialNumber,
                Code = Code,
                YearOfManufacture = YearOfManufacture,
                MillingProperties = new MillingProperties(),
                DrillPropeties = new DrillProperties(),
                EstimatorProperties = new EstimatorProperties(),
                SharedFolderPath = SharedFodlerPath
            };
        }
    }
}
