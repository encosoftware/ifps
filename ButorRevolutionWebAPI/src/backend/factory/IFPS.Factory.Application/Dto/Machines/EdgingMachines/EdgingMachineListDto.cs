using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class EdgingMachineListDto : MachineListDto
    {
        public EdgingMachineListDto() { }
        public EdgingMachineListDto(EdgingMachine machine) : base(machine) { }

        public static Expression<Func<EdgingMachine, EdgingMachineListDto>> Projection => entity => new EdgingMachineListDto
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
