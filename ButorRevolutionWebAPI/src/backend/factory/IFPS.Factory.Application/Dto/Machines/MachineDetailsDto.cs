using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MachineDetailsDto
    {
        public string MachineName { get; set; }
        public string SoftwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Code { get; set; }
        public int YearOfManufacture { get; set; }
        public int BrandId { get; set; }

        public MachineDetailsDto() { }
        public MachineDetailsDto(Machine machine)
        {
            BrandId = machine.BrandId.Value;
            MachineName = machine.Name;
            SoftwareVersion = machine.SoftwareVersion;
            SerialNumber = machine.SerialNumber;
            Code = machine.Code;
            YearOfManufacture = machine.YearOfManufacture;
        }
    }
}
