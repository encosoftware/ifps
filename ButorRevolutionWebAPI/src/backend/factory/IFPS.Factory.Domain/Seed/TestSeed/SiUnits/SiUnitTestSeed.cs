using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class SiUnitTestSeed : IEntitySeed<SiUnit>
    {
        public SiUnit[] Entities => new[]
        {
            new SiUnit(SiUnitEnum.M2) { Id = 10000 },
            new SiUnit(SiUnitEnum.Pcs) { Id = 10001 }
        };
    }
}
