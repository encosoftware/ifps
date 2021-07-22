using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class CuttingMachineListDto : MachineListDto
    {
        public CuttingMachineListDto() { }
        public CuttingMachineListDto(CuttingMachine machine) : base(machine) { }

        public static Expression<Func<CuttingMachine, CuttingMachineListDto>> Projection => entity => new CuttingMachineListDto
        {
            Id = entity.Id,
            MachineName = entity.Name,
            SoftwareVersion = entity.SoftwareVersion,
            SerialNumber = entity.SerialNumber,
            Code = entity.Code,
            YearOfManufacture = entity.YearOfManufacture,
            BrandName = entity.Brand.Name
        };
    }
}
