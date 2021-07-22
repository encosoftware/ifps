using IFPS.Factory.Domain.Constants;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class SiUnitSeed : IEntitySeed<SiUnit>
    {
        public SiUnit[] Entities => new[]
        {
            new SiUnit(SiUnitEnum.M) { Id = SiUnitConstants.MeterSiUnitId },
            new SiUnit(SiUnitEnum.Pcs) { Id = SiUnitConstants.PieceSiUnitId },
            new SiUnit(SiUnitEnum.Pkg) { Id = SiUnitConstants.PackageSiUnitId },
            new SiUnit(SiUnitEnum.M2) { Id = SiUnitConstants.SquareMeterUnitId }
        };
    }
}
