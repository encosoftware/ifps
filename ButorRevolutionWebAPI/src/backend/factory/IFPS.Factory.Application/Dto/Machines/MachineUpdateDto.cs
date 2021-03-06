using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class MachineUpdateDto
    {
        public string MachineName { get; set; }
        public string SoftwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Code { get; set; }
        public int YearOfManufacture { get; set; }
        public int BrandId { get; set; }
    }
}
