using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class MachineListDto
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string SoftwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Code { get; set; }
        public int YearOfManufacture { get; set; }
        public string BrandName { get; set; }

        public MachineListDto() { }

        public MachineListDto(Machine machine)
        {
            Id = machine.Id;
            MachineName = machine.Name;
            SoftwareVersion = machine.SoftwareVersion;
            SerialNumber = machine.SerialNumber;
            Code = machine.Code;
            YearOfManufacture = machine.YearOfManufacture;
            BrandName = machine.Brand.Name;
        }
    }
}
