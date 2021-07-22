using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class CncMachineUpdateDto : MachineUpdateDto
    {
        public string SharedFolderPath { get; set; }
        public CncMachine UpdateModelObject(CncMachine machine)
        {
            machine.Name = MachineName;
            machine.SoftwareVersion = SoftwareVersion;
            machine.SerialNumber = SerialNumber;
            machine.Code = Code;
            machine.YearOfManufacture = YearOfManufacture;
            machine.BrandId = BrandId;
            machine.SharedFolderPath = SharedFolderPath;
            return machine;
        }
    }
}
