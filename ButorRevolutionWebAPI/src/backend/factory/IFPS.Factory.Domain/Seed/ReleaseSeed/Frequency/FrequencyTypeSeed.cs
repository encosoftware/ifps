using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class FrequencyTypeSeed : IEntitySeed<FrequencyType>
    {
        public FrequencyType[] Entities => new[]
        {
            new FrequencyType(FrequencyTypeEnum.Day) { Id = 1 },
            new FrequencyType(FrequencyTypeEnum.Week) { Id = 2 },
            new FrequencyType(FrequencyTypeEnum.Month) { Id = 3 },
            new FrequencyType(FrequencyTypeEnum.Year) { Id = 4 },
        };
    }
}
