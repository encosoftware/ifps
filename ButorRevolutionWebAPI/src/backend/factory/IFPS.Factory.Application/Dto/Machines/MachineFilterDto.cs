using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class MachineFilterDto : OrderedPagedRequestDto
    {
        public string MachineName { get; set; }
        public string SoftwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Code { get; set; }
        public int YearOfManufacture { get; set; }
        public int BrandId { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(MachineName), nameof(Machine.Name) },
                { nameof(SoftwareVersion), nameof(Machine.SoftwareVersion) },
                { nameof(SerialNumber), nameof(Machine.SerialNumber) },
                { nameof(Code), nameof(Machine.Code) },
                { nameof(YearOfManufacture), nameof(Machine.YearOfManufacture) },
                { nameof(BrandId), nameof(Machine.BrandId) },
            };
        }
    }
}
