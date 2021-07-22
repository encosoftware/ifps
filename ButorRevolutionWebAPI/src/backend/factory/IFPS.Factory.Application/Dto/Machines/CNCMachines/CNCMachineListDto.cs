using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class CncMachineListDto : MachineListDto
    {
        public CncMachineListDto() { }
        public CncMachineListDto(CncMachine machine) : base(machine) { }
        public string SharedFolderPath { get; set; }

        public static Expression<Func<CncMachine, CncMachineListDto>> Projection => entity => new CncMachineListDto
        {
            Id = entity.Id,
            MachineName = entity.Name,
            SoftwareVersion = entity.SoftwareVersion,
            SerialNumber = entity.SerialNumber,
            Code = entity.Code,
            YearOfManufacture = entity.YearOfManufacture,
            BrandName = entity.Brand.Name,
            SharedFolderPath = entity.SharedFolderPath
        };
    }
}
