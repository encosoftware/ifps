using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class SortingStrategyTypeSeed : IEntitySeed<SortingStrategyType>
    {
        public SortingStrategyType[] Entities => new[]
        {
            new SortingStrategyType(SortingStrategyTypeEnum.BY_AREA_DESC) { Id = 1 },
            new SortingStrategyType(SortingStrategyTypeEnum.BY_ORDER_ASC) { Id = 2 }
        };
    }
}
